using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.States
{
    public class RejectedState : IArticleState
    {
        private readonly string _reason;

        public RejectedState(string reason)
        {
            _reason = reason;
        }

        public string Reason => _reason;

        public void Publish(Article article)
        {
            // Отклоненную статью нельзя опубликовать
            throw new InvalidOperationException("Отклоненную статью нельзя опубликовать");
        }

        public void Draft(Article article)
        {
            // Можно вернуть отклоненную статью в черновики для доработки
            article.SetState(new DraftState());
        }

        public void SendToModeration(Article article)
        {
            // Можно повторно отправить на модерацию после исправлений
            article.SetState(new ModerationState());
        }

        public void Reject(Article article, string reason)
        {
            // Обновить причину отклонения
            article.SetState(new RejectedState(reason));
        }

        public void Block(Article article, string reason)
        {
            article.SetState(new BlockedState(reason));
        }

        public bool CanLike() => false;
        public bool CanComment() => false;
        public bool CanEdit() => true;

        public string GetStateName() => "Отклонена";
    }
}
