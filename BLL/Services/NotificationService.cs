using AutoMapper;
using BLL.Models;
using DAL.Models;
using DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class NotificationService
    {
        private readonly UnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public NotificationService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BLNotification>> GetAllNotifications()
        {
            var dbNotifications =  await _unitOfWork.NotificationRepository.GetAsync();
            var blNotifications = _mapper.Map<IEnumerable<BLNotification>>(dbNotifications);
            return blNotifications;
        }

        public async Task<BLNotification> GetNotificationById(int id)
        {
            var dbNotifications =  await _unitOfWork.NotificationRepository.GetAsync();
            var dbNotification = dbNotifications.FirstOrDefault(n => n.Id == id);

            var blNotification = _mapper.Map<Models.BLNotification>(dbNotification);
            return blNotification;
        }

        public async Task AddNotification(BLNotification blNotification)
        {
            var notification = _mapper.Map<Notification>(blNotification);
            await _unitOfWork.NotificationRepository.InsertAsync(notification);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateNotification(BLNotification blNotification)
        {
            var existingNotification = await _unitOfWork.NotificationRepository.GetByIDAsync(blNotification.Id);

            if (existingNotification == null)
            {
                return;
            }

            _mapper.Map(blNotification, existingNotification);

            await _unitOfWork.NotificationRepository.UpdateAsync(existingNotification);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateNotificationAdmin(BLNotification blNotification)
        {
            var existingNotification = await _unitOfWork.NotificationRepository.GetByIDAsync(blNotification.Id);

            if (existingNotification == null)
            {
                return;
            }
            existingNotification.Subject = blNotification.Subject;
            existingNotification.Body = blNotification.Body;
            existingNotification.SentAt = blNotification.SentAt;

            _mapper.Map(blNotification, existingNotification);

            await _unitOfWork.NotificationRepository.UpdateAsync(existingNotification);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteNotification(int id)
        {
            await _unitOfWork.NotificationRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BLNotification> GetNotificationByMail(string mail,string subject)
        {
            var dbNotifications = await _unitOfWork.NotificationRepository.GetAsync();
            var dbNotification = dbNotifications.FirstOrDefault(n => n.ReceiverEmail == mail && n.Subject == subject);

            return _mapper.Map<BLNotification>(dbNotification);
        }


        public void SaveNotificationData() => _unitOfWork.Save();
    }
}
