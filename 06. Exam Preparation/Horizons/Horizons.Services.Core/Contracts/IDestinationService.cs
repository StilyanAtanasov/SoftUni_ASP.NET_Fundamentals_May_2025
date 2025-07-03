using Horizons.Services.Core.Utils;
using Horizons.Web.ViewModels.Destination;

namespace Horizons.Services.Core.Contracts
{
    public interface IDestinationService
    {
        Task<ICollection<DestinationCardViewModel>> GetDestinationCardsReadOnlyAsync(string? userId);

        Task AddDestination(DestinationFormViewModel model, string userId);

        Task<DestinationDetailsViewModel?> GetDestinationDetailsReadonlyAsync(int id, string? userId);

        Task<DestinationDeleteDetailsViewModel?> GetDestinationDeleteDetailsReadonlyAsync(int id);

        Task<ServiceResult> DeleteDestinationAsync(int id, string userId);

        Task<ServiceResult> EditDestinationAsync(DestinationFormViewModel model, string userId);

        Task<DestinationFormViewModel?> GetDestinationAsync(int id);

        Task<ICollection<DestinationFavoriteCardViewModel>> GetFavoriteDestinationsReadonlyAsync(string userId);

        Task<ServiceResult> AddFavoriteDestinationAsync(int destinationId, string userId);

        Task<ServiceResult> RemoveFavoriteDestinationAsync(int destinationId, string userId);
    }
}
