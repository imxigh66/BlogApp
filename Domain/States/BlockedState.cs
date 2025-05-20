using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.States
{
    public class BlockedState : IArticleState
    {
        private readonly string _reason;

        public BlockedState(string reason)
        {
            _reason = reason;
        }

        public string Reason => _reason;

        public void Publish(Article article)
        {
            // Заблокированную статью нельзя опубликовать
            throw new InvalidOperationException("Заблокированную статью нельзя опубликовать");
        }

        public void Draft(Article article)
        {
            // Можно восстановить из блокировки в черновик
            article.SetState(new DraftState());
            article.IsPublished = false;
        }

        public void SendToModeration(Article article)
        {
            // Заблокированную статью нельзя отправить на модерацию
            throw new InvalidOperationException("Заблокированную статью нельзя отправить на модерацию");
        }

        public void Reject(Article article, string reason)
        {
            // Заблокированную статью нельзя отклонить
            throw new InvalidOperationException("Заблокированную статью нельзя отклонить");
        }

        public void Block(Article article, string reason)
        {
            // Обновить причину блокировки
            article.SetState(new BlockedState(reason));
        }

        public bool CanLike() => false;
        public bool CanComment() => false;
        public bool CanEdit() => false;

        public string GetStateName() => "Заблокирована";
    }
}
