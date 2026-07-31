using Microsoft.AspNetCore.Mvc;
using RemindersDTI.DTOs;
using RemindersDTI.Services;

namespace RemindersDTI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Define a rota base para o controlador api/reminders
    public class RemindersController : ControllerBase
    {
        private readonly IReminderService _reminderService;

        // injeçao de dependencia do service
        public RemindersController(IReminderService service)
        {
            _reminderService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReminders()
        {
            var reminders = await _reminderService.GetAllAsync();
            return Ok(reminders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReminder([FromBody] CreateReminderDto reminderDto)
        {
            var reminder = await _reminderService.CreateAsync(reminderDto);
            return Created($"/api/reminders/{reminder.Id}", reminder); // Retorna o status 201 +  o lemnbrete
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteReminder(Guid id)
        {
            var result = await _reminderService.DeleteAsync(id);
            if(!result)
                return NotFound(); // Retorna 404 se o lembrete não for encontrado

            return NoContent(); // Retorna 204 se o lembrete foi deletado
        }
    }
}
