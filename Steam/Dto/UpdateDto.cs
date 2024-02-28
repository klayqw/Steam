﻿using System.ComponentModel.DataAnnotations;

namespace Steam.Dto;

public class UpdateDto
{
    public string AvatarUrl { get; set; }
    public string Password { get; set; }
    public string OldPassword { get; set; }
}
