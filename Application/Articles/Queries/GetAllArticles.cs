using Application.Articles.Dto;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.Queries
{
    public class GetAllArticles:IRequest<List<ArticleDto>>
    {
        public int? UserId { get; set; }
        public string SortingStrategy { get; set; } = "newest";
    }
}
