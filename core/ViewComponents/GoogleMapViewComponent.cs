using System.ComponentModel.DataAnnotations;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

[Display(Name = "Google Map", Prompt = "google-map")]
public class GoogleMapViewComponent : BaseViewComponent<GoogleMap>
{
    public GoogleMapViewComponent(IWorkService workService) : base(workService)
    {
    }
}
