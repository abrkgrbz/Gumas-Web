using System.ComponentModel.DataAnnotations;

namespace Gumas.Web.ViewModels;

public class ContactViewModel
{
    [Required(ErrorMessage = "Ad Soyad alanı zorunludur.")]
    [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir.")]
    [Display(Name = "Ad Soyad")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta alanı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    [Display(Name = "Telefon")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Konu alanı zorunludur.")]
    [StringLength(200, ErrorMessage = "Konu en fazla 200 karakter olabilir.")]
    [Display(Name = "Konu")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mesaj alanı zorunludur.")]
    [StringLength(2000, ErrorMessage = "Mesaj en fazla 2000 karakter olabilir.")]
    [Display(Name = "Mesaj")]
    public string Message { get; set; } = string.Empty;

    public bool IsSuccess { get; set; }
    public string? SuccessMessage { get; set; }
}
