namespace SkillForge.BusinessLogic.Services
{
     public class PasswordOptions
     {
          public int RequiredLength { get; internal set; }
          public bool RequireLowercase { get; internal set; }
          public bool RequireUppercase { get; internal set; }
          public bool RequireNonLetterOrDigit { get; internal set; }
          public bool RequireDigit { get; internal set; }
     }
}