﻿using Application.Articles.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.Queries
{
    public class GetPendingArticlesQuery:IRequest<List<ArticleDto>>
    {
    }
}
