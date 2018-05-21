using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jQueryAjaxDemo.Entities;

namespace jQueryAjaxDemo.BusinessLogic
{
    public class UIMethods
    {
        private jQueryAjaxDemoEntities _context = new jQueryAjaxDemoEntities();

        public static bool UserLikes(string userId, Guid imageId)
        {
           var _context = new jQueryAjaxDemoEntities();
            var image = _context.Images.FirstOrDefault(i => i.ImageId == imageId);
            return image.UserLikes.Any(ul => ul.Id == userId);

        }

    }
}