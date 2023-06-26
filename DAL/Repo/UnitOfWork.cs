using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public class UnitOfWork : IDisposable
    {
        private RwaMoviesContext? context = new RwaMoviesContext();
        private GenericRepository<Video>? videoRepository;
        private GenericRepository<Genre>? genreRepository;
        private GenericRepository<Tag>? tagRepository;
        private GenericRepository<Notification>? notificationRepository;
        private GenericRepository<Country>? countryRepository;
        private GenericRepository<User>? userRepository;
        private GenericRepository<Image>? imageRepository;
        private GenericRepository<VideoTag>? videoTagRepository;

        public GenericRepository<Video> VideoRepository
        {
            get
            {

                if (this.videoRepository == null)
                {
                    this.videoRepository = new GenericRepository<Video>(context);
                }
                return videoRepository;
            }
        }

        public GenericRepository<Genre> GenreRepository
        {
            get
            {

                if (this.genreRepository == null)
                {
                    this.genreRepository = new GenericRepository<Genre>(context);
                }
                return genreRepository;
            }
        }

        public GenericRepository<Tag> TagRepository
        {
            get
            {

                if (this.tagRepository == null)
                {
                    this.tagRepository = new GenericRepository<Tag>(context);
                }
                return tagRepository;
            }
        }

        public GenericRepository<Notification> NotificationRepository
        {
            get
            { 
                if (this.notificationRepository == null)
                {
                    this.notificationRepository = new GenericRepository<Notification>(context);
                }
                return notificationRepository;
            }
        }

        public GenericRepository<Country> CountryRepository
        {
            get
            {

                if (this.countryRepository == null)
                {
                    this.countryRepository = new GenericRepository<Country>(context);
                }
                return countryRepository;
            }
        }

        public GenericRepository<User> UserRepository
        {
            get
            {

                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<User>(context);
                }
                return userRepository;
            }
        }

        public GenericRepository<Image> ImageRepository
        {
            get
            {

                if (this.imageRepository == null)
                {
                    this.imageRepository = new GenericRepository<Image>(context);
                }
                return imageRepository;
            }
        }

        public GenericRepository<VideoTag> VideoTagRepository
        {
            get
            {
                if (this.videoTagRepository == null)
                {
                    this.videoTagRepository = new GenericRepository<VideoTag>(context);
                }
                return videoTagRepository;
            }
        }

        public async Task SaveAsync()
        {
           await context?.SaveChangesAsync();
        }

        public void Save()
        {
            context?.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context?.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
