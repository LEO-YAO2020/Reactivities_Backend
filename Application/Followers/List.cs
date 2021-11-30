using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers
{
    public class List
    {
        public class Query : IRequest<Result<List<Profile.Profile>>>
        {
            public string Predicate { get; set; }
            public string Username { get; set; }
        }
        
        public class Handler : IRequestHandler<Query, Result<List<Profile.Profile>>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IMapper mapper,IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }
            public async Task <Result<List<Profile.Profile>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profile = new List<Profile.Profile>();
                switch (request.Predicate)
                {
                    case "followers":
                        profile = await _dataContext.UserFollowings
                            .Where(x => x.Target.UserName == request.Username)
                            .Select(u => u.Observer)
                            .ProjectTo<Profile.Profile>(_mapper.ConfigurationProvider,
                                new {currentUsername = _userAccessor.GetUsername()})
                            .ToListAsync();
                        break;
                    case "following":
                        profile = await _dataContext.UserFollowings
                            .Where(x => x.Observer.UserName == request.Username)
                            .Select(u => u.Target)
                            .ProjectTo<Profile.Profile>(_mapper.ConfigurationProvider,
                                new {currentUsername = _userAccessor.GetUsername()})
                            .ToListAsync();
                        break;
                }

                return Result<List<Profile.Profile>>.Success(profile);
            }
        }
    }
}