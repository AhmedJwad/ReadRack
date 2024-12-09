using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using ReadRack.Shared.Entites;

namespace ReadRack.Fronend.Pages.Colleges
{
    public partial class CollegeForm
    {
        [Parameter] public College college { get; set; }
        [Parameter] public EventCallback OnValidSubmit { get; set; }

    }
}
