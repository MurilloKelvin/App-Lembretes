using RemindersDTI.DTOs;

namespace RemindersDTI.Services
{
    public interface IReminderService
    {
        Task<IEnumerable<ReminderDto>> GetAllAsync();
        Task<ReminderDto> CreateAsync(CreateReminderDto reminderDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
