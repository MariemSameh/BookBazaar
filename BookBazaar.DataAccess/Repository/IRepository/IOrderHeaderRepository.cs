using BookBazaar.Models;

namespace BookBazaar.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
        void UpdateStatus(int id, string OrderStatus, string? PaymentStatus = null);
        void UpdateStriprPaymentId (int id, string sessionId, string paymentIntentId);
    }
}
