using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alldux_plataforma.Models
{
    public static class Log
    {
        public static Guid Id { get; set; } = new Guid();
        public static Guid UserId { get; set; }
        public static DateTime Data { get; set; } = DateTime.Now;
        public static string Status { get; set; }
        public static string Msg { get; set; }
        public static string More { get; set; }

    }
}