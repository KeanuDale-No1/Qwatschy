using Microsoft.AspNetCore.Mvc;
using VoiceChat.Api.Services;
using VoiceChat.Api.UseCases;
using VoiceChat.Data.Repositories;
using VoiceChat.Shared.Models;

namespace VoiceChat.Api.Endpoints
{
    public static class ChannelEndpoints
    {
        public static void AddChannelEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/CreateChannel", async (IUseCase<CreateChannelRequestDTO, CreateChannelResponseDTO> useCase, CreateChannelRequestDTO request) =>
            {
                return await useCase.ExecuteAsync(request);
            });

            app.MapPost("api/GetChannels", async (IUseCase<GetChannelsRequestDTO, GetChannelsResponseDTO> useCase, GetChannelsRequestDTO request) =>
            {
                return await useCase.ExecuteAsync(request);
            });

            app.MapPost("api/DeleteChannel", async (IUseCase<DeleteChannelRequestDTO, DeleteChannelResponseDTO> useCase, DeleteChannelRequestDTO request) =>
            {
                return await useCase.ExecuteAsync(request);
            });
        }
    }
}
