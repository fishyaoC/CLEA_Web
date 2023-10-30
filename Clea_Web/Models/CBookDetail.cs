using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CBookDetail
{
    public Guid MdId { get; set; }

    public Guid MId { get; set; }

    public Guid MdPublish { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}
