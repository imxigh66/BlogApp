﻿using Application.Articles.Dto;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.Queries
{
    public class GetArticlesById:IRequest<ArticleDetailDto>
    {
        public int ArticleId { get; set; }  
    }
}
