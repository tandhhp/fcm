using Microsoft.AspNetCore.Identity;
using OpenAI;
using OpenAI.Chat;
using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Ocsp;
using SendGrid.Helpers.Mail;
using System;
using Waffle.Controllers;
using Waffle.Core.Interfaces.IService;
using Waffle.Models;

namespace Waffle.Core.Services;

public class AIService : IAIService
{
    private readonly ISettingService _settingService;
    private readonly ILogger<AIService> _logger;
    private const string CHATGPT_SETTING_KEY = "CHATGPT";
    private readonly IConfiguration _configuration;

    public AIService(ISettingService settingService, ILogger<AIService> logger, IConfiguration configuration)
    {
        _settingService = settingService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<TResult<string>> ChatAsync(List<ChatRequest> requests)
    {
        try
        {
            var apiKey = _configuration["CHATGPT_API_KEY"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return TResult<string>.Failed("ChatGPT chưa được cấu hình");
            }

            var client = new OpenAIClient(apiKey);
            var chat = client.GetChatClient("gpt-4.1-mini");
            var chatMessages = new List<ChatMessage>();
            var assMessage = new List<AssistantChatMessage>();
            foreach (var req in requests)
            {
                if (req.Role.Equals("user", StringComparison.OrdinalIgnoreCase))
                {
                    chatMessages.Add(ChatMessage.CreateUserMessage(req.Content));
                }
                if (req.Role.Equals("assistant", StringComparison.OrdinalIgnoreCase))
                {
                    chatMessages.Add(ChatMessage.CreateAssistantMessage(req.Content));
                }
            }
            var response = await chat.CompleteChatAsync(chatMessages);
            var aiResponse = response.Value.Content.FirstOrDefault()?.Text;
            return TResult<string>.Ok(aiResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling ChatGPT API");
            return TResult<string>.Failed($"Có lỗi xảy ra: {ex.Message}");
        }
    }

    public async Task<TResult<string>> ChatWithContextAsync(string content, string? context)
    {
        try
        {
            var apiKey = _configuration["CHATGPT_API_KEY"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return TResult<string>.Failed("ChatGPT chưa được cấu hình");
            }

            var client = new OpenAIClient(apiKey);
            
            var systemPrompt = string.IsNullOrWhiteSpace(context)
                ? "Bạn là trợ lý AI, hãy trả lời hỗ trợ người dùng một cách chuyên nghiệp."
                : $"Bạn là trợ lý AI, chỉ trả lời dựa trên dữ liệu cung cấp sau đây:\n{context}\n\nHãy trả lời hỗ trợ người dùng một cách chuyên nghiệp.";

            var chatMessage = ChatMessage.CreateUserMessage(content);
            var chat = client.GetChatClient("gpt-4.1-mini");

            var response = await chat.CompleteChatAsync(chatMessage);

            var aiResponse = response.Value.Content.FirstOrDefault()?.Text;
            return TResult<string>.Ok(aiResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling ChatGPT API with context");
            return TResult<string>.Failed($"Có lỗi xảy ra: {ex.Message}");
        }
    }
}