using System.ComponentModel.DataAnnotations;

namespace Models.Dtos;

public record class CreateCodingTimeDto(string Task, string StartTime, string EndTime)
{
}

