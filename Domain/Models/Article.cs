

using Domain.Models.Content;
using Domain.States;

namespace Domain.Models
{
    public class Article : ICloneable<Article>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public User Author { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsPublished { get; set; } = false; // По умолчанию статья на модерации
        public List<Like> Likes { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();

        public List<Content.Content> ContentItems { get; set; } = new List<Content.Content>();


        // Добавляем поле для состояния
        [NonSerialized]
        private IArticleState _state;

        // Добавляем поле для хранения названия состояния в БД
        public string StateName { get; set; } = "Draft";

        // Добавляем поле для причин отклонения/блокировки
        public string StateReason { get; set; }

        public Article()
        {
            _state = new DraftState();
        }

        // Метод для установки состояния
        public void SetState(IArticleState state)
        {
            _state = state;
            StateName = state.GetStateName();

            // Если состояние содержит причину, сохраняем её
            if (state is BlockedState blockedState)
            {
                StateReason = blockedState.Reason;
            }
            else if (state is RejectedState rejectedState)
            {
                StateReason = rejectedState.Reason;
            }
        }

        // Получить текущее состояние
        public IArticleState GetState()
        {
            // Если _state не инициализировано (например, после загрузки из БД),
            // восстанавливаем его на основе строкового имени
            if (_state == null)
            {
                RestoreState();
            }

            return _state;
        }

        // Восстановление объекта состояния из строкового имени
        private void RestoreState()
        {
            switch (StateName)
            {
                case "Черновик":
                    _state = new DraftState();
                    break;
                case "На модерации":
                    _state = new ModerationState();
                    break;
                case "Опубликована":
                    _state = new PublishedState();
                    break;
                case "Заблокирована":
                    _state = new BlockedState(StateReason);
                    break;
                case "Отклонена":
                    _state = new RejectedState(StateReason);
                    break;
                default:
                    _state = new DraftState();
                    break;
            }
        }

        // Делегирование методов состоянию
        public void Publish()
        {
            GetState().Publish(this);
        }

        public void Draft()
        {
            GetState().Draft(this);
        }

        public void SendToModeration()
        {
            GetState().SendToModeration(this);
        }

        public void Reject(string reason)
        {
            GetState().Reject(this, reason);
        }

        public void Block(string reason)
        {
            GetState().Block(this, reason);
        }

        // Проверка возможности действий
        public bool CanLike() => GetState().CanLike();
        public bool CanComment() => GetState().CanComment();
        public bool CanEdit() => GetState().CanEdit();
        public Article Clone()
        {
            return new Article
            {
                Title = this.Title + " (копия)",
                Content = this.Content,
                AuthorId = this.AuthorId,
                CreatedAt = DateTime.UtcNow,
                IsPublished = false
            };
        }

        public Article CloneWithDetails()
        {
            var clone = this.Clone();
            return clone;
        }
    }
}
