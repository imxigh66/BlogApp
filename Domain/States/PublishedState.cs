using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.States
{
    public class PublishedState : IArticleState
    {
        public void Publish(Article article)
        {
            // Уже опубликована
        }

        public void Draft(Article article)
        {
            article.SetState(new DraftState());
            article.IsPublished = false;
        }

        public void SendToModeration(Article article)
        {
            article.SetState(new ModerationState());
            article.IsPublished = false;
        }

        public void Reject(Article article, string reason)
        {
            // Опубликованную статью нельзя отклонить, только отправить в черновики
            throw new InvalidOperationException("Нельзя отклонить опубликованную статью");
        }

        public void Block(Article article, string reason)
        {
            article.SetState(new BlockedState(reason));
            article.IsPublished = false;
        }

        public bool CanLike() => true;
        public bool CanComment() => true;
        public bool CanEdit() => true;

        public string GetStateName() => "Опубликована";
    }
}
