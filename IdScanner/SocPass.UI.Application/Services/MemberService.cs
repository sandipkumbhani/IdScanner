
using Microsoft.Extensions.Logging;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }
        public async Task<List<Member>> GetAllMember(int flatId)
        {
            var result = await _memberRepository.GetAllMemberAsync(flatId);
            return result ?? new List<Member>();
        }
        public async Task<string> AddMemberAsync(MemberCreateRequest memberCreateRequest)
        {
            var result = await _memberRepository.AddMemberAsync(memberCreateRequest);
            return result;
        
        }
        public async Task<string>GeneratePass(int blockId, int EventId)
        {
            return await _memberRepository.GeneratePass(blockId, EventId);
        }
        public async Task<string> GenerateGuestPass(int blockId, int EventId)
        {
            return await _memberRepository.GenerateGuestPass(blockId, EventId);
        }
        public async Task<QRCodeMaster> GetMemberByMemberId(int memberId,int EventId)
        {
           return await _memberRepository.GetMemberByMemberId(memberId, EventId);
        }
        public async Task<string> AddAndUpdateGuestAsync(MemberCreateRequest memberCreateRequest)
        {
            var result = await _memberRepository.AddAndUpdateGuestAsync(memberCreateRequest);
            return result;

        }
        public async Task<List<Member>> GetAllMemberAsync(int flatId)
        {
            var result = await _memberRepository.GetAllMemberAsync(flatId);
            return result ?? new List<Member>();
        }
    }
}
