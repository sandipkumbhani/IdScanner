using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QRCoder;
using SocPass.Application.Interface;
using SocPass.Domain.DTO;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Application.Services
{

    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IFlatRepository _flatRepository;
        private readonly AppSettingsDTO _baseUrl;
        public MemberService(IMemberRepository memberRepository, IFlatRepository flatRepository, IOptions<AppSettingsDTO> baseUrl)
        {
            _memberRepository = memberRepository;
            _flatRepository = flatRepository;
            _baseUrl = baseUrl.Value;
        }
        public async Task CreateAndUpdateMemberAsync(int flatId, int numberOfAdults, List<int> childAges)
        {
            var flat = await _flatRepository.GetById(flatId);
            if (flat == null)
            {
                throw new Exception("Flat not found");
            }
            flat.NumberOfAdult = numberOfAdults;
            flat.NumberOfChild = childAges.Count;
            flat.TotalMember = numberOfAdults + childAges.Count;
            flat.UpdateBy = 1;
            flat.UpdateDate = DateTime.Now;

            await _flatRepository.UpdateFlatAsync(flat);

            var members = await _memberRepository.GetById(flatId);
            var adults = members.Where(m => !m.IsChild).ToList();
            var children = members.Where(m => m.IsChild).ToList();

            if (adults.Count() > numberOfAdults)
            {
                foreach (var extra in adults.Skip(numberOfAdults))
                {
                    extra.IsActive = false;
                    extra.UpdateBy = 1;
                    extra.UpdateDate = DateTime.Now;
                    await _memberRepository.UpdateMemberAsync(extra);
                }
            }
            else if (adults.Count() < numberOfAdults)
            {
                for (int i = adults.Count(); i < numberOfAdults; i++)
                {
                    var newAdult = new Member
                    {
                        FlatId = flatId,
                        IsChild = false,
                        ChildAge = 0,
                        IsGuest = false,
                        IsActive = true,
                        InsertBy = 1,
                        InsertDate = DateTime.Now,
                        UpdateBy = 1,
                        UpdateDate = DateTime.Now

                    };
                    var result = await _memberRepository.AddMemberAsync(newAdult);
                    //await updatedQrAsync(result.MemberId, result.IsChild);
                }
            }
            if (children.Count() > childAges.Count)
            {
                foreach (var extra in children.Skip(childAges.Count))
                {
                    extra.IsActive = false;
                    extra.UpdateBy = 1;
                    extra.UpdateDate = DateTime.Now;
                    await _memberRepository.UpdateMemberAsync(extra);
                }
            }
            for (int i = 0; i < childAges.Count; i++)
            {
                if (i < children.Count())
                {
                    var child = children[i];
                    child.ChildAge = childAges[i];
                    child.IsActive = true;
                    child.UpdateBy = 1;
                    child.UpdateDate = DateTime.Now;
                    await _memberRepository.UpdateMemberAsync(child);
                }
                else
                {
                    var newChild = new Member
                    {
                        FlatId = flatId,
                        IsChild = true,
                        ChildAge = childAges[i],
                        IsGuest = false,
                        IsActive = true,
                        InsertBy = 1,
                        InsertDate = DateTime.Now,
                        UpdateBy = 1,
                        UpdateDate = DateTime.Now
                    };
                    var result = await _memberRepository.AddMemberAsync(newChild);
                    //await updatedQrAsync(result.MemberId, result.IsChild);
                }
            }
        }
        public async Task<List<Member>> GetGuestByIdAsync(int flatId)
        {
            var getGuest = await _memberRepository.GetGuestsByFlatIdAsync(flatId);
            return getGuest ?? new List<Member>();
        }
        public async Task<List<Member>> GetMemberByIdAsync(int flatId)
        {
            var members = await _memberRepository.GetById(flatId);
            return members ?? new List<Member>();
        }
        public async Task CreateAndUpdateGuestAsync(int flatId, int numberOfAdults, List<int> childAges)
        {
            var guests = await _memberRepository.GetGuestsByFlatIdAsync(flatId);

            if (guests == null)
                throw new Exception("Guest members not found");

            var existingAdults = guests.Where(x => !x.IsChild).ToList();
            var existingChildren = guests.Where(x => x.IsChild).ToList();

            if (existingAdults.Count >= numberOfAdults)
            {
                foreach (var adult in existingAdults.Take(numberOfAdults))
                {
                    adult.UpdateBy = 1;
                    adult.UpdateDate = DateTime.Now;
                    await _memberRepository.UpdateMemberAsync(adult);
                }
                foreach (var extraAdult in existingAdults.Skip(numberOfAdults))
                {
                    await _memberRepository.DeleteMemberAsync(extraAdult.MemberId);
                }
            }
            else
            {
                foreach (var adult in existingAdults)
                {
                    adult.UpdateBy = 1;
                    adult.UpdateDate = DateTime.Now;
                    await _memberRepository.UpdateMemberAsync(adult);
                }

                int adultsToAdd = numberOfAdults - existingAdults.Count;
                for (int i = 0; i < adultsToAdd; i++)
                {
                    var newAdult = new Member
                    {
                        FlatId = flatId,
                        IsChild = false,
                        ChildAge = 0,
                        IsGuest = true,
                        IsActive = true,
                        InsertBy = 1,
                        InsertDate = DateTime.Now,
                        UpdateBy = 1,
                        UpdateDate = DateTime.Now
                    };
                    var result = await _memberRepository.AddMemberAsync(newAdult);
                    //await updatedQrAsync(result.MemberId, result.IsChild);
                }
            }

            if (existingChildren.Count >= childAges.Count)
            {
                for (int i = 0; i < childAges.Count; i++)
                {
                    var child = existingChildren[i];
                    child.ChildAge = childAges[i];
                    child.UpdateBy = 1;
                    child.UpdateDate = DateTime.Now;
                    await _memberRepository.UpdateMemberAsync(child);
                }
                foreach (var extraChild in existingChildren.Skip(childAges.Count))
                {
                    await _memberRepository.DeleteMemberAsync(extraChild.MemberId);
                }
            }
            else
            {
                for (int i = 0; i < existingChildren.Count; i++)
                {
                    var child = existingChildren[i];
                    child.ChildAge = childAges[i];
                    child.UpdateBy = 1;
                    child.UpdateDate = DateTime.Now;
                    await _memberRepository.UpdateMemberAsync(child);
                }
                for (int i = existingChildren.Count; i < childAges.Count; i++)
                {
                    var newChild = new Member
                    {
                        FlatId = flatId,
                        IsChild = true,
                        ChildAge = childAges[i],
                        IsGuest = true,
                        IsActive = true,
                        InsertBy = 1,
                        InsertDate = DateTime.Now,
                        UpdateBy = 1,
                        UpdateDate = DateTime.Now
                    };
                    var result = await _memberRepository.AddMemberAsync(newChild);
                    //await updatedQrAsync(result.MemberId, result.IsChild);
                }
            }
        }
        public async Task<string> AddMemberPassDateAsync(int blockId, int eventId)
        {
            var (existingQRGenerated, flatsList, members) = await _memberRepository.AddMemberPassDateAsync(blockId, eventId);
            var qrList = new List<QRCodeMaster>();

            foreach (var member in members)
            {
                if (!existingQRGenerated.Any(x => x.MemberId == member.MemberId))
                {
                    var flat = flatsList.FirstOrDefault(f => f.FlatId == member.FlatId);
                    if (flat == null) continue;

                    string societyName = flat.Block?.Society?.Name ?? "UnknownSociety";
                    string blockNumber = flat.Block?.BlockNumber?.ToString() ?? "Block";
                    string flatNumber = flat.FlatNumber?.ToString() ?? member.FlatId.ToString();

                    string qrRelativeUrl = await GenerateAndStoreQrAsync(
                        member.MemberId,
                        member.IsChild,
                        societyName,
                        blockNumber,
                        flatNumber,
                        eventId.ToString());

                    qrList.Add(new QRCodeMaster
                    {
                        MemberId = member.MemberId,
                        EventId = eventId,
                        QRCodeUrl = qrRelativeUrl,
                        IsActive = true,
                        Visited = false,
                        InsertBy = 1,
                        UpdateBy = 1,
                        InsertDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    });
                }
            }

            if (qrList.Any())
            {
                await _memberRepository.AddQrMasterAsync(qrList);
            }

            return "QR Codes added successfully!";
        }
        public async Task<string> GenerateGuestQRAsync(int blockId, int eventId)
        {
            var (existingQRGenerated, flatsList, members) = await _memberRepository.GenerateGuestQRAsync(blockId, eventId);
            var qrList = new List<QRCodeMaster>();

            foreach (var member in members)
            {
                if (!existingQRGenerated.Any(x => x.MemberId == member.MemberId))
                {
                    var flat = flatsList.FirstOrDefault(f => f.FlatId == member.FlatId);
                    if (flat == null) continue;

                    string societyName = flat.Block?.Society?.Name ?? "UnknownSociety";
                    string blockNumber = flat.Block?.BlockNumber?.ToString() ?? "Block";
                    string flatNumber = flat.FlatNumber?.ToString() ?? member.FlatId.ToString();

                    string qrRelativeUrl = await GenerateAndStoreQrAsync(
                        member.MemberId,
                        member.IsChild,
                        societyName,
                        blockNumber,
                        flatNumber,
                        eventId.ToString());

                    qrList.Add(new QRCodeMaster
                    {
                        MemberId = member.MemberId,
                        EventId = eventId,
                        QRCodeUrl = qrRelativeUrl,
                        IsActive = true,
                        Visited = false,
                        InsertBy = 1,
                        UpdateBy = 1,
                        InsertDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    });
                }
            }
            if (qrList.Any())
            {
                await _memberRepository.AddQrMasterAsync(qrList);
            }
            return "QR Codes added successfully!";
        }
        public async Task<QRCodeMaster> GetMemberByMemberId(int memberId, int EventId)
        {
            var getMember = await _memberRepository.GetMemberByMemberIdAsync(memberId, EventId);
            if (getMember == null)
            {
                throw new KeyNotFoundException($"Member with ID {memberId} not found.");
            }

            return getMember;
        }
        public async Task<string> IsVisitedAsync(int memberid, int EventId, int loggedInUserId)
        {
            return await _memberRepository.IsVisitedAsync(memberid, EventId, loggedInUserId);
        }
        private async Task<string> GenerateAndStoreQrAsync(int memberId, bool isChild, string societyName, string blockNumber, string flatNumber, string EventId)
        {
            //string qrContentUrl = $"{_baseUrl.BaseUrl}/MemberDetails/GetDetails/{memberId}/{EventId}";
            string rawData = $"{memberId}|{EventId}";
            string encoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(rawData));
            string qrContentUrl = $"{_baseUrl.BaseUrl}/MemberDetails/GetDetails/{encoded}";

            //string qrContentUrl = $"http://localhost:5109/MemberDetails/GetDetails/{memberId}";
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrData = qrGenerator.CreateQrCode(qrContentUrl, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new PngByteQRCode(qrData))
            {
                byte[] qrBytes = qrCode.GetGraphic(20);

                using (var ms = new MemoryStream(qrBytes))
                using (var originalBitmap = new Bitmap(ms))
                using (var bitmap = new Bitmap(originalBitmap.Width, originalBitmap.Height, PixelFormat.Format32bppArgb))
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.White);
                    graphics.DrawImage(originalBitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

                    string label = isChild ? "CHILD" : "ADULT";
                    using (var font = new Font("Arial", 14, FontStyle.Bold))
                    using (var brush = new SolidBrush(Color.Red))
                    {
                        SizeF textSize = graphics.MeasureString(label, font);
                        float rectX = (bitmap.Width - textSize.Width) / 2 - 10;
                        float rectY = (bitmap.Height - textSize.Height) / 2 - 5;
                        float rectWidth = textSize.Width + 20;
                        float rectHeight = textSize.Height + 10;

                        graphics.FillRectangle(Brushes.White, rectX, rectY, rectWidth, rectHeight);
                        graphics.DrawString(label, font, brush,
                            new RectangleF(rectX, rectY, rectWidth, rectHeight),
                            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    string contentRoot = Directory.GetCurrentDirectory();
                    string wwwroot = Path.Combine(@"D:\Broadsy\Project\IdScanner\SocPass.UI", "wwwroot");
                
                    string nestedFolder = Path.Combine(wwwroot, "QRCodes",
                                                      EventId,
                                                      societyName,
                                                      blockNumber,
                                                      flatNumber);
                    if (!Directory.Exists(nestedFolder))
                    {
                        Directory.CreateDirectory(nestedFolder);
                    }
                    string qrFileName = $"{memberId}_qr.png";
                    string fullPhysicalPath = Path.Combine(nestedFolder, qrFileName);
                    bitmap.Save(fullPhysicalPath, ImageFormat.Png);

                    string qrRelativeUrl = $"/QRCodes/{Uri.EscapeDataString(EventId)}/{Uri.EscapeDataString(societyName)}/" +
                                           $"{Uri.EscapeDataString(blockNumber)}/" +
                                           $"{Uri.EscapeDataString(flatNumber)}/" +
                                           qrFileName;
                    //var member = await _context.QRCodeMasters.FindAsync(memberId);
                    //if (member != null)
                    //{
                    //    member.QRCodeUrl = qrRelativeUrl;
                    //    _context.QRCodeMasters.Update(member);
                    //    await _context.SaveChangesAsync();
                    //}
                    return qrRelativeUrl;
                }
            }
        }
    }
}
