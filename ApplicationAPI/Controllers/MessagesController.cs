

using System.Reflection;
using ApplicationAPI.Data;
using ApplicationAPI.DTOs;
using ApplicationAPI.Entities;
using ApplicationAPI.Extensions;
using ApplicationAPI.Helpers;
using ApplicationAPI.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationAPI.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfwork;
        private readonly IMapper _mapper;
        public MessagesController(IUnitOfWork unitOfwork, IMapper mapper)
        {
            _unitOfwork = unitOfwork;
            _mapper = mapper;
        }
        
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();
            
            if(username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("Users can't send messages to themselves");
            
            var sender = await _unitOfwork.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await _unitOfwork.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if(recipient == null)
                return NotFound();
            
            var message = new Message
            {
               Sender = sender,
               Recipient = recipient,
               SenderUsername = sender.UserName,
               RecipientUsername = recipient.UserName,
               Content =  createMessageDto.Content
            };

            _unitOfwork.MessageRepository.AddMessage(message);
            if(await _unitOfwork.Complete())
                return Ok(_mapper.Map<MessageDto>(message));
            return BadRequest("Failed to send message.");
        }
        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessageForUser([FromQuery]MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await _unitOfwork.MessageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PagesSize, messages.TotalCount, messages.TotalPages));
            return messages;
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();
            var message = await _unitOfwork.MessageRepository.GetMessage(id);

            if(message.SenderUsername != username && message.RecipientUsername != username)
                return Unauthorized();
            if(message.SenderUsername == username)
                message.SenderDeleted = true;
            if(message.RecipientUsername == username)
               message.RecipientDeleted = true;
            if(message.SenderDeleted && message.RecipientDeleted)
                _unitOfwork.MessageRepository.DeleteMessage(message); 

            if(await _unitOfwork.Complete())
                return Ok();
            return BadRequest("Problem deleting message");
        }
    }
}