using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;

namespace Waffle.Models.Components;

[Display(Name = "Testimonial", Prompt = "testimonial")]
public class Testimonial : AbstractComponent
{
    [JsonPropertyName("title")]
    public string? Title { get; set; } = "Trải nghiệm của khách hàng";
    [JsonPropertyName("items")]
    public List<TestimonialItem> Items { get; set; } = new();
}

public class TestimonialItem
{
    public string? Avatar { get; set; }
    public string? Name { get; set; }
    public string? Message { get; set; }
}

public class TestimonialDemoData
{
    public static Testimonial Data = new()
    {
        Title = "Trải nghiệm của khách hàng",
        Items = new List<TestimonialItem>
        {
            new()
            {
                Avatar = "https://i.imgur.com/ttFgN84.jpeg",
                Name = "Chị Thảo Nguyễn",
                Message = "Tôi rất hài lòng với những gì NURA'S thay tôi chăm sóc cho Bố mẹ, từ những điều nhỏ nhất đến việc chăm lo cho sức khỏe của Bố mẹ tôi mà ko có từ nào có thể diễn tả hết cảm xúc này. Cảm ơn Nura’s cùng tôi thực hiện giấc mơ giúp Bố mẹ khỏe mạnh hơn từng ngày."
            },
            new()
            {
                Avatar = "https://i.imgur.com/FXMhXD1.png",
                Name = "Cô Mai Vũ",
                Message = @"Cô nay là 54 tuổi, lần đầu tiên có ngươì hỗ trợ chăm sóc tận tình khi đi xét nghiệm. Các em trợ lý cá nhân ở NURA'S hướng dẫn rất là nhiệt tình, chăm sóc rất là kỹ lưỡng. 
                            Cô cảm ơn NURA'S đã tạo điều kiện cho Cô và gia đình hiểu rõ hơn về sức khỏe và có biện pháp hỗ trợ tốt nhất. "
            },
            new()
            {
                Avatar = "https://i.imgur.com/Jrj1RXM.png",
                Name = "Gia đình chú Lâm",
                Message = "Đây là lần thứ 2 chúng tôi sử dụng dịch vụ, cũng như những lần trước, lần này chúng tôi rất hài lòng, cách hỗ trợ khách hàng của NURA'S rất chu đáo: chu đáo về tất cả mọi mặt, về dịch vụ, sản phẩm, nhân viên đều rất hài lòng."
            }
        }
    };
}