using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.States
{
    public interface IArticleState
    {
        // Методы для изменения состояния
        void Publish(Article article);
        void Draft(Article article);
        void SendToModeration(Article article);
        void Reject(Article article, string reason);
        void Block(Article article, string reason);

        // Методы для проверки доступных действий
        bool CanLike();
        bool CanComment();
        bool CanEdit();

        // Получить название состояния
        string GetStateName();
    }
}
