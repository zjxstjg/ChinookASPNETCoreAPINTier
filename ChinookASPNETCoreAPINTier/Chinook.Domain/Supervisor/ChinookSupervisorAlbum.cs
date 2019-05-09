using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Chinook.Domain.Extensions;
using Chinook.Domain.ApiModels;
using Chinook.Domain.Converters;
using Chinook.Domain.Entities;

namespace Chinook.Domain.Supervisor
{
    public partial class ChinookSupervisor
    {
        public async Task<IEnumerable<AlbumApiModel>> GetAllAlbumAsync(CancellationToken ct = default)
        {
            var albums = await _albumRepository.GetAllAsync(ct);
            return albums.ConvertAll();
        }

        public async Task<AlbumApiModel> GetAlbumByIdAsync(int id, CancellationToken ct = default)
        {
            var albumApiModel = (await _albumRepository.GetByIdAsync(id, ct)).Convert;
            albumApiModel.ArtistName = (await _artistRepository.GetByIdAsync(albumApiModel.ArtistId, ct)).Name;
            return albumApiModel;
        }

        public async Task<IEnumerable<AlbumApiModel>> GetAlbumByArtistIdAsync(int id, 
            CancellationToken ct = default)
        {
            var albums = await _albumRepository.GetByArtistIdAsync(id, ct);
            return albums.ConvertAll();
        }

        public async Task<AlbumApiModel> AddAlbumAsync(AlbumApiModel newAlbumApiModel,
            CancellationToken ct = default)
        {
            var album = new Album
            {
                Title = newAlbumApiModel.Title,
                ArtistId = newAlbumApiModel.ArtistId
            };

            album = await _albumRepository.AddAsync(album, ct);
            newAlbumApiModel.AlbumId = album.AlbumId;
            return newAlbumApiModel;
        }

        public async Task<bool> UpdateAlbumAsync(AlbumApiModel albumApiModel,
            CancellationToken ct = default)
        {
            var album = await _albumRepository.GetByIdAsync(albumApiModel.AlbumId, ct);

            if (album is null) return false;
            album.AlbumId = albumApiModel.AlbumId;
            album.Title = albumApiModel.Title;
            album.ArtistId = albumApiModel.ArtistId;

            return await _albumRepository.UpdateAsync(album, ct);
        }

        public Task<bool> DeleteAlbumAsync(int id, CancellationToken ct = default) 
            => _albumRepository.DeleteAsync(id, ct);
    }
}