using SocPass.Application.Interface;
using SocPass.Domain.DTO;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Application.Services
{
    public class FlatService : IFlatService
    {
        private readonly IFlatRepository _flatRepository;
        public FlatService(IFlatRepository flatRepository)
        {
            _flatRepository = flatRepository;
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
            var result = await _flatRepository.GetAllFlatAsync();
            if (result == null)
            {
                throw new KeyNotFoundException("List not Found");

            }
            return result;
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
        public async Task<List<Flat>>   GetFlatByBlockID(int blockid)
        {
            return await _flatRepository.GetFlatByBlockIdAsync(blockid);
        }
        public async Task<List<FlatWithMembersDto>> GetMemberQr(int blockid)
        {
            return await _flatRepository.GetMemberQr(blockid);
        }
        public async Task<List<FlatWithMembersDto>> GetGuestQr(int blockid)
        {
            return await _flatRepository.GetGuestQr(blockid);
        }
    }
}
