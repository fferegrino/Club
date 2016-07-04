using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using AutoMapper;
using Club.Common;
using Club.Common.TypeMapping;
using Club.Models.Entities;

namespace Club.AutoMappings
{
    public class ClubUserToSimpleUserTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap<Club.Models.Entities.ClubUser,Club.ViewModels.SimpleUserViewModel>()
                .ForMember(model => model.Username, options => options.MapFrom(viewModel => viewModel.UserName))
                .ForMember(model => model.Level, options => options.Ignore())
                ;

            Mapper.CreateMap<Club.Models.Entities.ClubUser, Club.ViewModels.LetterViewModel>()
                .ForMember(l => l.Ano, opt => opt.Ignore())
                .ForMember(l => l.Mes, opt => opt.Ignore())
                .ForMember(l => l.Dias, opt => opt.Ignore())
                .ForMember(l => l.NombreAlumno, opt => opt.ResolveUsing(m => m.LastName + " " + m.FirstName))
                .ForMember(l => l.Periodos, opt => opt.ResolveUsing<PeriodosConverter>())
                .ForMember(l => l.Horas, opt => opt.ResolveUsing<HorasConverter>())
                ;

        }
    }

    public class HorasConverter : ValueResolver<Models.Entities.ClubUser, string>
    {
        protected override string ResolveCore(ClubUser source)
        {
            var horus = source.EventsAttended.Sum(evt => (evt.Event.End - evt.Event.Start).TotalHours);
            var hs = (int)Math.Round(horus);

            return "hora".ToQuantity(hs);
        }
    }

    public class PeriodosConverter : ValueResolver<Models.Entities.ClubUser, string>
    {
        protected override string ResolveCore(ClubUser source)
        {
            var terms = source.EventsAttended.GroupBy(evt => evt.Event.TermId);
            List<Term> t = new List<Term>();
            foreach (var term in terms)
            {
                t.Add(term.First().Event.Term);
            }
            return t.OrderBy(r0 => r0.Start)
                .Select(trm => $"{trm.Start:MMMM yyy} - {trm.End:MMMM yyy}")
                .Humanize();
        }
    }

}
