namespace RemindersDTI.DTOs
{
    public class CreateReminderDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime ReminderDate { get; set; }
    }
}
