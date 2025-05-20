using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.States
{
    public class DraftState : IArticleState
    {
        public void Publish(Article article)
        {
            // Автор имеет высокий рейтинг и может публиковать без модерации
            article.SetState(new PublishedState());
            article.IsPublished = true;
        }

        public void Draft(Article article)
        {
            // Уже в состоянии черновика
        }

        public void SendToModeration(Article article)
        {
            article.SetState(new ModerationState());
        }

        public void Reject(Article article, string reason)
        {
            // Нельзя отклонить черновик
            throw new InvalidOperationException("Нельзя отклонить статью в состоянии черновика");
        }

        public void Block(Article article, string reason)
        {
            article.SetState(new BlockedState(reason));
        }

        public bool CanLike() => false;
        public bool CanComment() => false;
        public bool CanEdit() => true;

        public string GetStateName() => "Черновик";
    }
}
