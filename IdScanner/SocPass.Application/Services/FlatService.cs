using Microsoft.AspNetCore.Http;
using SocPass.Application.Interface;
using SocPass.Domain.DTO;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using System.Security.Claims;
namespace SocPass.Application.Services
{
    public class FlatService : IFlatService
    {
        private readonly IFlatRepository _flatRepository;
        private readonly ISocietyRepository _societyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FlatService(IFlatRepository flatRepository,IHttpContextAccessor httpContextAccessor,ISocietyRepository societyRepository)
        {
            _flatRepository = flatRepository;
            _societyRepository = societyRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Flat>> CreateFlatAsync(Flat flat)
        {
            var createdFlats = new List<Flat>();

            if (flat.StartFlatNumber == 0 && flat.EndFlatNumber == 0 && flat.NumberOfFlats == 0)
            {
                var newFlat = new Flat
                { 
                    SocietyId = flat.SocietyId,
                    BlockId = flat.BlockId,
                    FlatNumber = flat.FlatNumber.ToString(),
                    FloorNumber = flat.FloorNumber,
                    TotalMember = flat.TotalMember,
                    NumberOfAdult = flat.NumberOfAdult,
                    NumberOfChild = flat.NumberOfChild,
                    IsActive = true,
                    InsertBy = 1,
                    InsertDate = DateTime.Now,
                    UpdateBy = 1,
                    UpdateDate = DateTime.Now
                };

                var result = await _flatRepository.CreateFlatAsync(newFlat);
                createdFlats.Add(result);
                return createdFlats;
            }
            if (flat.EndFlatNumber < flat.StartFlatNumber)
            {
                throw new ArgumentException("End flat number must be >= start flat number.");
            }
            if (flat.StartFlatNumber >= 1 && flat.StartFlatNumber <= 99)
            {
                int totalFlats = flat.EndFlatNumber - flat.StartFlatNumber + 1;
                for (int i = 0; i < totalFlats; i++)
                {
                    int floor = (i / flat.NumberOfFlats) + 1;
                    int flatIndex = (i % flat.NumberOfFlats) + 1;
                    int flatNumber = flat.StartFlatNumber + i;
                    var newFlat = new Flat
                    {
                        SocietyId = flat.SocietyId,
                        BlockId = flat.BlockId,
                        FlatNumber = flatNumber.ToString(),
                        NumberOfFlats = flat.NumberOfFlats,
                        FloorNumber = floor,
                        IsActive = true,
                        InsertBy = 1,
                        InsertDate = DateTime.Now,
                        UpdateBy = 1,
                        UpdateDate = DateTime.Now
                    };
                    var result = await _flatRepository.CreateFlatAsync(newFlat);
                    createdFlats.Add(newFlat);
                }
            }
            else
            {
                int suffixLength = flat.StartFlatNumber.ToString().Length - 1;
                int baseSuffix = flat.StartFlatNumber % (int)Math.Pow(10, suffixLength);
                int startFloor = flat.StartFlatNumber / (int)Math.Pow(10, suffixLength);
                for (int floor = startFloor; floor < startFloor + flat.FloorNumber; floor++)
                {
                    for (int flatIndex = 0; flatIndex < flat.NumberOfFlats; flatIndex++)
                    {
                        int flatNumber = floor * (int)Math.Pow(10, suffixLength) + baseSuffix + flatIndex;
                        if (flatNumber > flat.EndFlatNumber)
                            break;
                        var newFlat = new Flat
                        {
                            SocietyId = flat.SocietyId,
                            BlockId = flat.BlockId,
                            FlatNumber = flatNumber.ToString(),
                            NumberOfFlats = flat.NumberOfFlats,
                            TotalMember = flat.TotalMember,
                            NumberOfAdult = flat.NumberOfAdult,
                            NumberOfChild = flat.NumberOfChild,
                            FloorNumber = floor,
                            IsActive = true,
                            InsertBy = 1,
                            InsertDate = DateTime.Now,
                            UpdateBy = 1,
                            UpdateDate = DateTime.Now
                        };
                        var result = await _flatRepository.CreateFlatAsync(newFlat);
                        createdFlats.Add(result);
                    }
                }
            }
            return createdFlats;
        }
        public async Task<List<Flat>> GetAllFlatAsync()
        {
            var flatList = (await _flatRepository.GetAllFlatAsync()).ToList();
            string role = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            var FlatList = new List<Flat>();
            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                FlatList = await _flatRepository.GetAllFlatAsync();
            }
            else
            {
                var societies = await _societyRepository.GetAllSocietyAsync(userId);
                var society = societies.FirstOrDefault();

                if (society != null)
                {
                    FlatList = flatList
                                .Where(e => e.SocietyId == society.SocietyId)
                                .ToList();
                }
                else
                {
                    FlatList = new List<Flat>();
                }
            }
            return FlatList ?? new List<Flat>();
        }
        public async Task<Flat> GetByIdAsync(int flatid)
        {
            var result = await _flatRepository.GetById(flatid);
            if (result == null)
            {
                throw new KeyNotFoundException($"Flat with Id {flatid} not found");

            }
            return result;
        }
        public async Task<List<Flat>> UpdateFlatAsync(Flat flat)
        {
            int i = 0;
            var updatedFlats = new List<Flat>();

            if (flat.EndFlatNumber < flat.StartFlatNumber)
            {
                throw new ArgumentException("End flat number must be >= start flat number.");
            }

            var existingFlatsManual = await _flatRepository.GetFlatsByBlockAsync(flat.SocietyId, flat.BlockId);
            var orderedExisting = existingFlatsManual.OrderBy(f => f.FlatId).ToList();

            if (flat.StartFlatNumber >= 1 && flat.StartFlatNumber <= 99)
            {
                int totalFlats = flat.EndFlatNumber - flat.StartFlatNumber + 1;

                for (i = 0; i < totalFlats; i++)
                {
                    int floor = (i / flat.NumberOfFlats) + 1;
                    int flatNumber = flat.StartFlatNumber + i;

                    if (i < orderedExisting.Count)
                    {
                        var existingFlat = orderedExisting[i];
                        existingFlat.FlatNumber = flatNumber.ToString();
                        existingFlat.FloorNumber = floor;
                        existingFlat.NumberOfFlats = flat.NumberOfFlats;
                        existingFlat.UpdateBy = 1;
                        existingFlat.UpdateDate = DateTime.Now;

                        await _flatRepository.UpdateAsync(existingFlat);
                        updatedFlats.Add(existingFlat);
                    }
                    else
                    {
                        var newFlat = new Flat
                        {
                            SocietyId = flat.SocietyId,
                            BlockId = flat.BlockId,
                            FlatNumber = flatNumber.ToString(),
                            NumberOfFlats = flat.NumberOfFlats,
                            FloorNumber = floor,
                            IsActive = true,
                            InsertBy = 1,
                            InsertDate = DateTime.Now,
                            UpdateBy = 1,
                            UpdateDate = DateTime.Now
                        };
                        await _flatRepository.CreateFlatAsync(newFlat);
                        updatedFlats.Add(newFlat);
                    }
                }
                if (orderedExisting.Count > totalFlats)
                {
                    var flatsToRemove = orderedExisting.Skip(totalFlats).ToList();
                    foreach (var f in flatsToRemove)
                    {
                        await _flatRepository.DeleteFlatAsync(f.FlatId);
                    }
                }
            }
            else if (flat.StartFlatNumber == 0 && flat.EndFlatNumber == 0 && flat.NumberOfFlats == 0)
            {
                var existingFlat = await _flatRepository.GetFlatByNumberAsync(flat.SocietyId, flat.BlockId, int.Parse(flat.FlatNumber));
                if (existingFlat == null)
                {
                    var newFlat = new Flat
                    {
                        SocietyId = flat.SocietyId,
                        BlockId = flat.BlockId,
                        FlatNumber = flat.FlatNumber.ToString(),
                        FloorNumber = flat.FloorNumber,
                        TotalMember = flat.TotalMember,
                        NumberOfAdult = flat.NumberOfAdult,
                        NumberOfChild = flat.NumberOfChild,
                        IsActive = true,
                        InsertBy = 1,
                        InsertDate = DateTime.Now,
                        UpdateBy = 1,
                        UpdateDate = DateTime.Now
                    };

                    var result = await _flatRepository.CreateFlatAsync(newFlat);
                    updatedFlats.Add(result);
                    return updatedFlats;
                }
                else
                {
                    if(existingFlat.SocietyId == flat.SocietyId && existingFlat.BlockId == flat.BlockId && existingFlat.FloorNumber == flat.FloorNumber && existingFlat.FlatNumber == flat.FlatNumber)
                    {
                        throw new InvalidOperationException("Flat number already exists in the same society and block.");
                    }
                }
            }
            else
            {
                int digitLength = flat.StartFlatNumber.ToString().Length - 1;
                int baseSuffix = flat.StartFlatNumber % (int)Math.Pow(10, digitLength);
                int startFloor = flat.StartFlatNumber / (int)Math.Pow(10, digitLength);
                int endFloor = flat.EndFlatNumber / (int)Math.Pow(10, digitLength);
                for (int floor = startFloor; floor <= endFloor; floor++)
                {
                    for (int flatIndex = 0; flatIndex < flat.NumberOfFlats; flatIndex++)
                    {
                        int flatNumber = floor * (int)Math.Pow(10, digitLength) + baseSuffix + flatIndex;
                        if (i < orderedExisting.Count)
                        {
                            var existingFlat = orderedExisting[i];
                            existingFlat.FlatNumber = flatNumber.ToString();
                            existingFlat.FloorNumber = floor;
                            existingFlat.NumberOfFlats = flat.NumberOfFlats;
                            existingFlat.UpdateBy = 1;
                            existingFlat.UpdateDate = DateTime.Now;
                            await _flatRepository.UpdateAsync(existingFlat);
                            updatedFlats.Add(existingFlat);
                        }
                        else
                        {
                            var newFlat = new Flat
                            {
                                SocietyId = flat.SocietyId,
                                BlockId = flat.BlockId,
                                FlatNumber = flatNumber.ToString(),
                                NumberOfFlats = flat.NumberOfFlats,
                                TotalMember = flat.TotalMember,
                                NumberOfAdult = flat.NumberOfAdult,
                                NumberOfChild = flat.NumberOfChild,
                                FloorNumber = floor,
                                IsActive = true,
                                InsertBy = 1,
                                InsertDate = DateTime.Now,
                                UpdateBy = 1,
                                UpdateDate = DateTime.Now
                            };
                            var result = await _flatRepository.CreateFlatAsync(newFlat);
                            updatedFlats.Add(result);
                        }
                        i++;
                    }
                }
                if (orderedExisting.Count > i)
                {
                    var flatsToRemove = orderedExisting.Skip(i).ToList();
                    foreach (var f in flatsToRemove)
                    {
                        await _flatRepository.DeleteFlatAsync(f.FlatId);
                    }
                }
            }
            return updatedFlats;
        }
        public async Task<List<Flat>>  GetFlatByBlockID(int blockid)
        {
            return await _flatRepository.GetFlatByBlockIdAsync(blockid);
        }
        public async Task<List<FlatWithMembersDto>> GetMemberQr(int blockid, int eventId)
        {
            return await _flatRepository.GetMemberQrAsync(blockid, eventId);
        }
        public async Task<List<FlatWithMembersDto>> GetGuestQr(int blockid, int EventId)
        {
            return await _flatRepository.GetGuestQr(blockid, EventId);
        }
    }
}
