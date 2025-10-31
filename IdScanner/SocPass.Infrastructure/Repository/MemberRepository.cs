using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using QRCoder;
using SocPass.Domain.DTO;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using SocPass.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Infrastructure.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _context;
        private readonly AppSettingsDTO _baseUrl;
        public MemberRepository(AppDbContext context, IOptions<AppSettingsDTO> baseUrl)
        {
            _baseUrl = baseUrl.Value;
            _context = context;
        }
        public async Task<Member> AddMemberAsync(Member member)
        {
            _context.members.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }
        public async Task UpdateQrCodeAsync(int memberId, string qrCodeUrl)
        {
            var user = await _context.members.FindAsync(memberId);
            if (user != null)
            {
                user.QRCodeUrl = qrCodeUrl;
                _context.members.Update(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Member>> GetGuestsByFlatIdAsync(int flatId)
        {
            return await _context.members
                .Where(x => x.FlatId == flatId && x.IsGuest == true)
                .ToListAsync();
        }
        public async Task<List<Member>> GetById(int flatId)
        {
            return await _context.members
                .Where(x => x.IsActive && !x.IsGuest && x.FlatId == flatId)
                .ToListAsync();
        }
        public async Task UpdateMemberAsync(Member member)
        {
            _context.members.Update(member);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteMemberAsync(int memberId)
        {
            var member = await _context.members.FindAsync(memberId);
            if (member != null)
            {
                member.IsActive = false;
                await _context.SaveChangesAsync();
            }
            ;
        }
        public async Task<Member?> GetMemberByMemberIdAsync(int memberId, int eventId)
        {
            return await _context.QRCodeMasters
                .Where(x => x.MemberId == memberId && x.EventId == eventId && x.IsActive)
                .Select(m => new Member
                {
                    MemberId = m.MemberId,
                    IsChild = m.Member.IsChild,
                    ChildAge = m.Member.ChildAge,
                    IsGuest = m.Member.IsGuest,
                    FlatId = m.Member.FlatId,
                    Visited = m.Visited,
                    Flat = m.Member.Flat != null ? new Flat
                    {
                        FlatId = m.Member.Flat.FlatId,
                        FlatNumber = m.Member.Flat.FlatNumber,
                        TotalMember = m.Member.Flat.TotalMember,
                        NumberOfChild = m.Member.Flat.NumberOfChild,
                        NumberOfAdult = m.Member.Flat.NumberOfAdult,
                        Block = m.Member.Flat.Block != null ? new Block
                        {
                            BlockId = m.Member.Flat.Block.BlockId,
                            BlockNumber = m.Member.Flat.Block.BlockNumber
                        } : null,
                        Society = m.Member.Flat.Society != null ? new Society
                        {
                            SocietyId = m.Member.Flat.Society.SocietyId,
                            Name = m.Member.Flat.Society.Name
                        } : null
                    } : null
                })
                .FirstOrDefaultAsync();
        }


        public async Task<bool> IsVisitedAsync(int memberid, int loggedInUserId)
        {
            var entity = await _context.members
    .FirstOrDefaultAsync(x => x.MemberId == memberid);

            if (entity == null)
                return false;

            if (!entity.Visited)
            {
                entity.Visited = true;
                entity.UpdateDate = DateTime.Now;
                entity.UpdateBy = loggedInUserId;
                await _context.SaveChangesAsync();
            }
            return true;

        }
        public async Task<bool> AddGuestPassDateAsync(int blockId, DateTime passDate)
        {
            var flatIds = await _context.flats
                .Where(f => f.BlockId == blockId && f.IsActive)
                .Select(f => f.FlatId)
                .ToListAsync();
            if (!flatIds.Any())
                return false;

            var members = await _context.members
                .Where(m => flatIds.Contains(m.FlatId) && m.IsActive && m.IsGuest)
                .ToListAsync();
            if (!members.Any())
                return false;

            foreach (var member in members)
            {
                member.PassDate = DateOnly.FromDateTime(passDate);
                member.Visited = false;
                member.UpdateDate = DateTime.Now;

                var flat = await _context.flats
                    .Include(f => f.Block)
                    .ThenInclude(b => b.Society)
                    .FirstOrDefaultAsync(f => f.FlatId == member.FlatId);

                string societyName = flat?.Block?.Society?.Name ?? "UnknownSociety";
                string blockNumber = flat?.Block?.BlockNumber.ToString() ?? "Block";
                string flatNumber = flat?.FlatNumber.ToString() ?? member.FlatId.ToString();
                string passdate = member.PassDate.ToString();

                string qrRelativeUrl = await GenerateAndStoreQrAsync(
                    member.MemberId,
                    member.IsChild,
                    societyName,
                    blockNumber,
                    flatNumber, passdate);

            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddMemberPassDateAsync(int blockId, int EventId)
        {
            var flatIds = await _context.flats
                .Where(f => f.BlockId == blockId && f.IsActive)
                .Select(f => f.FlatId)
                .ToListAsync();

            if (!flatIds.Any())
                return false;

            var members = await _context.members
                .Where(m => flatIds.Contains(m.FlatId) && m.IsActive && !m.IsGuest)
                .ToListAsync();

            if (!members.Any())
                return false;

            var qrList = new List<QRCodeMaster>();

            foreach (var member in members)
            {
                var flat = await _context.flats
                    .Include(f => f.Block)
                    .ThenInclude(b => b.Society)
                    .FirstOrDefaultAsync(f => f.FlatId == member.FlatId);

                string societyName = flat?.Block?.Society?.Name ?? "UnknownSociety";
                string blockNumber = flat?.Block?.BlockNumber.ToString() ?? "Block";
                string flatNumber = flat?.FlatNumber.ToString() ?? member.FlatId.ToString();
                string qrRelativeUrl = await GenerateAndStoreQrAsync(
                    member.MemberId,
                    member.IsChild,
                    societyName,
                    blockNumber,
                    flatNumber,
                    EventId.ToString());

                qrList.Add(new QRCodeMaster
                {
                    MemberId = member.MemberId,
                    EventId = EventId,
                    QRCodeUrl = qrRelativeUrl,
                    IsActive = true,
                    Visited = true,
                    InsertBy = 1,
                    UpdateBy = 1,
                    InsertDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                });
            }

            _context.QRCodeMasters.AddRange(qrList);
            await _context.SaveChangesAsync();

            return true;
        }
        private async Task<string> GenerateAndStoreQrAsync(int memberId, bool isChild, string societyName, string blockNumber, string flatNumber, string EventId)
        {
            string qrContentUrl = $"{_baseUrl.BaseUrl}/MemberDetails/GetDetails/{memberId}?eve{EventId}";
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
                    string wwwroot = Path.Combine(@"D:\Broadsy\Projects\IdScanner\IdScanner\SocPass.UI", "wwwroot");

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


