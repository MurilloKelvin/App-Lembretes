using Microsoft.EntityFrameworkCore;
using RemindersDTI.Data;
using RemindersDTI.DTOs;
using RemindersDTI.Models;

namespace RemindersDTI.Services
{
    public class ReminderService : IReminderService
    {
        private readonly AppDbContext _context; // referência para o contexto do banco de dados

        public ReminderService(AppDbContext context) // injeção de dependência do contexto do banco de dados
        {
            _context = context;
        }

        public async Task<ReminderDto> CreateAsync(CreateReminderDto reminderDto)
        {
            var reminder = new Reminder
            {
                Id = Guid.NewGuid(),
                Name = reminderDto.Name,
                Description = reminderDto.Description,
                ReminderDate = reminderDto.ReminderDate
            };

            _context.Reminders.Add(reminder); // adiciona o lembrete
            await _context.SaveChangesAsync(); // salva as alterações no banco de dados

            return new ReminderDto // converte de Model para DTO antes de devolver
            {
                Id = reminder.Id,
                Name = reminder.Name,
                Description = reminder.Description,
                ReminderDate = reminder.ReminderDate
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var reminder = await _context.Reminders.FindAsync(id);
            if (reminder == null)
                return false; // Retorna falso se o lembrete não for encontrado
            
            _context.Reminders.Remove(reminder); // deleta o lembrete 
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ReminderDto>> GetAllAsync()
        {
            // Busca os lembretes no banco de dados ordenados pela data
            var reminders = await _context.Reminders
                .OrderBy(r => r.ReminderDate)
                .ToListAsync();

            // Converte de Model para DTO antes de devolver
            return reminders.Select(r => new ReminderDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                ReminderDate = r.ReminderDate
            });
        }
    }
}
