using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.States
{
    public class ModerationState : IArticleState
    {
        public void Publish(Article article)
        {
            // Только модератор может опубликовать из этого состояния
            article.SetState(new PublishedState());
            article.IsPublished = true;
        }

        public void Draft(Article article)
        {
            article.SetState(new DraftState());
            article.IsPublished = false;
        }

        public void SendToModeration(Article article)
        {
            // Уже на модерации
        }

        public void Reject(Article article, string reason)
        {
            article.SetState(new RejectedState(reason));
            article.IsPublished = false;
        }

        public void Block(Article article, string reason)
        {
            article.SetState(new BlockedState(reason));
            article.IsPublished = false;
        }

        public bool CanLike() => false;
        public bool CanComment() => false;
        public bool CanEdit() => false;

        public string GetStateName() => "На модерации";
    }
}
