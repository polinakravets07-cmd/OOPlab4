using System;
using System.Collections.Generic;
namespace SocialMedia
{
    struct Comment
    {
        public string Author;
        public string Text;
        public DateTime Date;
        public Comment(string author, string text)
        {
            Author = author;
            Text = text;
            Date = DateTime.Now;
        }
    }
 class Post
    {
        public int Id;
        public DateTime Date;
        public string Text;
        public List<string> Likes;
        public List<Comment> Comments;
        public Post(int id, string text)
        {
            Id = id;
            Text = text;
            Date = DateTime.Now;
            Likes = new List<string>();
            Comments = new List<Comment>();
        }
        public void ShowPost()
        {
            Console.WriteLine("\n----------------------------");
            Console.WriteLine("ID поста: " + Id);
            Console.WriteLine("Дата: " + Date);
            Console.WriteLine("Текст: " + Text);
            Console.WriteLine("Кількість лайків: " + Likes.Count);
            Console.WriteLine("Коментарі:");
            if (Comments.Count == 0)
                Console.WriteLine("Коментарів поки немає.");
            foreach (Comment comment in Comments)
            {
                Console.WriteLine(comment.Author + ": " + comment.Text);
                Console.WriteLine("Дата: " + comment.Date);
            }
            Console.WriteLine("----------------------------");
        }
    }
 class User
    {
        public string Name;
        public string Email;
        public string Nickname;
        public List<Post> Posts;
        public User(string name, string email, string nickname)
        {
            Name = name;
            Email = email;
            Nickname = nickname;
            Posts = new List<Post>();
        }
        // Створення публікації
        public void CreatePost(string text, int id)
        {
            Posts.Add(new Post(id, text));
            Console.WriteLine("Пост успішно створений!");
        }
        // Видалення публікації
        public void DeletePost(int id)
        {
            Post post = Posts.Find(p => p.Id == id);
            if (post != null)
            {
                Posts.Remove(post);
                Console.WriteLine("Пост видален!");
            }
            else
            {
                Console.WriteLine("Пост не знайдено!");
            }
        }
        // Перегляд останніх п'яти публікацій
        public void ShowLastFivePosts()
        {
            Console.WriteLine("\nОстанні публікації користувача" + Nickname);
            if (Posts.Count == 0)
            {
                Console.WriteLine("У користувача поки немає постів.");
                return;
            }
            int start = Math.Max(0, Posts.Count - 5);
            for (int i = Posts.Count - 1; i >= start; i--)
            {
                Posts[i].ShowPost();
            }
        }
    }
 class Program
    {
        static List<User> users = new List<User>();
        static int nextPostId = 1;
        static void Main(string[] args)
        {
            //Створення користувачів
            User user1 = new User(
                "Поліна Кравець",
                "polina@gmail.com",
                "@polina"
                );
            User user2 = new User(
                "Анастасія",
                "nastya@gmail.com",
                "@nastya"
                );
            User user3 = new User(
                "Олександр",
                "alex@gmail.com",
                "@alex"
                );
            users.Add(user1);
            users.Add(user2);
            users.Add(user3);
            // Додавання початкових публикацій
            user2.CreatePost("Привіт усім!", nextPostId++);
            user2.CreatePost("Сьогодні чудовий день!", nextPostId++);
            user2.CreatePost("Моя нова фотографія", nextPostId++);
            user2.CreatePost("Гарного всім вечора!", nextPostId++);
            user2.CreatePost("Нова публікація!", nextPostId++);
            user3.CreatePost("Всшм привіт!", nextPostId++);
            user3.CreatePost("Гарного дня!", nextPostId++);
            //Вибір поточного користувача
            User currentUser = user1;
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n===== Соціальна мережа =====");
                Console.WriteLine("Ви увійшли як: " + currentUser.Nickname);
                Console.WriteLine("1. Переглянути останні 5 публікацій користувача");
                Console.WriteLine("2. Поставити вподобання");
                Console.WriteLine("3. Написати коментар");
                Console.WriteLine("4. Створити нову публікацію");
                Console.WriteLine("5. Видалити свою публикацію");
                Console.WriteLine("6. Переглянути свої публікації");
                Console.WriteLine("0. Вхід");
                Console.Write("\nОберіть дію: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowOtherUserPosts(currentUser);
                        break;
                    case "2":
                        LikePost(currentUser);
                        break;
                    case "3":
                        AddComment(currentUser);
                        break;
                    case "4":
                        Console.Write("Введіть текст публікації: ");
                        string text = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(text))
                            currentUser.CreatePost(text, nextPostId++);
                        else
                            Console.WriteLine("Текст не може бути порожнім.");
                        break;
                    case "5":
                        Console.Write("Ведіть ID публікації: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteId))
                            currentUser.DeletePost(deleteId);
                        else
                            Console.WriteLine("Некоректний ID.");
                        break;
                    case "6":
                        currentUser.ShowLastFivePosts();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Вихід із соціальної мережі.");
                        break;
                    default:
                        Console.WriteLine("Неправильний вибір!");
                        break;
                }
            }
        }
        //Пошук користувача за нікнеймом
        static User FindUser(string nickname)
        {
            return users.Find(u => u.Nickname == nickname);
        }
        //Перегляд публікацій іншого користувача
        static void ShowOtherUserPosts(User currentUser)
        {
            Console.Write("Введіть нікнейм користувача: ");
            string nickname = Console.ReadLine();
            User user = FindUser(nickname);
            if (user == null || user == currentUser)
            {
                Console.WriteLine("Користувача не знайдено або вибрано власний профіль!");
                return;
            }
            user.ShowLastFivePosts();
        }
        //Додавання вподобання до публікації
        static void LikePost(User currentUser)
        {
            Post post = FindOtherPost(currentUser);
            if (post == null)
                return;
            if (post.Likes.Contains(currentUser.Nickname))
            {
                Console.WriteLine("Ви вже вподобали цю публікацію!");
                return;
            }
            post.Likes.Add(currentUser.Nickname);
            Console.WriteLine("Вподобання успішно додано!");
        }
        // Додавання коментаря до публікації
        static void AddComment(User currentUser)
        {
            Post post = FindOtherPost(currentUser);
            if (post == null)
                return;
            Console.Write("Введіть коментар: ");
            string text = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Коментар не може бути порожнім.");
                return;
            }
            post.Comments.Add(new Comment(currentUser.Nickname, text));
            Console.WriteLine("Коментар успішно додано!");
        }
        // Пошук публікації іншого користувача
        static Post FindOtherPost(User currentUser)
        {
            Console.WriteLine("Введіть нікнейм автора публікації: ");
            string nickname = Console.ReadLine();
            User user = FindUser(nickname);
            if (user == null || user == currentUser)
            {
                Console.WriteLine("Користувача не знайдено або це Ваш профіль!");
                return null;
            }
            Console.Write("Введіть ID публікації: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некоректний ID!");
                return null;
            }
            Post post = user.Posts.Find(p => p.Id == id);
            if (post == null)
                Console.WriteLine("Публікацію не знайдено!");
            return post;
        }
    }
}
