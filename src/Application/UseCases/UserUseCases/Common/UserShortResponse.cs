﻿namespace Application.UseCases.UserUseCases.Common
{
    public sealed record UserShortResponse
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
    }
}
