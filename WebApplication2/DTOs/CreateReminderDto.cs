using System.ComponentModel.DataAnnotations;

namespace RemindersDTI.DTOs
{
    public class CreateReminderDto
    {
        [Required(ErrorMessage = "O nome do lembrete é obrigatorio")]
        [StringLength(100, ErrorMessage = "O nome do lembrete não pode ter mais de 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição do lembrete não pode ter mais de 500 caracteres")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data do lembrete é obrigatoria")]
        public DateTime ReminderDate { get; set; }

        // Implementação da validação da data
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ReminderDate < DateTime.Now)
            {
                yield return new ValidationResult(
                    "A data do lembrete não pode ser no passado",
                    new[] { nameof(ReminderDate) });
            }
        }

    }
}
