namespace RemindersDTI.Models
{
    public class Reminder
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        
        public DateTime ReminderDate { get; set; }
    }
}
