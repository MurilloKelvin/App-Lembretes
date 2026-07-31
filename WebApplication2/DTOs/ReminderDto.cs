namespace RemindersDTI.DTOs
{
    public class ReminderDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime ReminderDate { get; set; }
    }
}
