﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Club.Common.TypeMapping;
using Club.ViewModels;
using Humanizer;

namespace Club.AutoMappings
{
    public class SubmissionsMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap<Club.ViewModels.SubmissionViewModel,Club.Models.Entities.Submission>()
                .ForMember(vm => vm.User, opt => opt.Ignore())
                .ForMember(vm => vm.Problem, opt => opt.Ignore())
                .ForMember(vm => vm.UserId, opt => opt.Ignore())
                .ForMember(vm => vm.Accepted, opt => opt.Ignore())
                .ForMember(vm => vm.LastAttemptDate, opt => opt.Ignore())
                .ForMember(vm => vm.File, opt => opt.Ignore())
                ;

            Mapper.CreateMap<Club.Models.Entities.Submission, Club.ViewModels.SubmissionViewModel>()
                .ForMember(en => en.User, opt => opt.ResolveUsing(ent => ent.User.UserName))
                .ForMember(en => en.ProblemId, opt => opt.ResolveUsing(ent => ent.Problem.Id))
                .ForMember(en => en.ProblemName, opt => opt.ResolveUsing(ent => ent.Problem.Name))
                .ForMember(en => en.GistId, opt => opt.ResolveUsing<GetGistIdValueResolver>())
                .ForMember(en => en.FileContent, opt => opt.ResolveUsing<FileBytesToStringResolver>())
                ;

            Mapper.CreateMap<Club.Models.Entities.Submission, Club.ApiModels.SubmissionApiModel>()
                .ForMember(en => en.User, opt => opt.ResolveUsing(ent => ent.User.UserName))
                .ForMember(en => en.ProblemId, opt => opt.ResolveUsing(ent => ent.Problem.Id))
                //.ForMember(en => en.Status, opt => opt.ResolveUsing(ent => ent.Accepted.HasValue == false ? "Pendiente" : (ent.Accepted.GetValueOrDefault() ? "Aceptado" : "Rechazado")))
                .ForMember(en => en.ProblemName, opt => opt.ResolveUsing(ent => ent.Problem.Name))
                ;

        }
    }

    public class GetGistIdValueResolver : ValueResolver<Club.Models.Entities.Submission, string>
    {
        protected override string ResolveCore(Club.Models.Entities.Submission source)
        {
            if (source?.GistUrl == null) return "";
            try
            {
                var uri = new Uri(source.GistUrl);
                return uri.Segments.Last();

            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }

    public class FileBytesToStringResolver : ValueResolver<Club.Models.Entities.Submission, string>
    {
        protected override string ResolveCore(Club.Models.Entities.Submission source)
        {
            if (source.File == null) return null;

            return System.Text.Encoding.UTF8.GetString(source.File);
        }
    }
}
