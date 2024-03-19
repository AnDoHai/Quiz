using System.Globalization;
using System.Text;
using Tms.Models.Resources;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using EcommerceSystem.Core.Configurations;

namespace System.Web.Mvc.Html
{
    public static class HtmlHelpers
    {
        public static HtmlString PagingHelper(this HtmlHelper htmlHelper, string actionName, string strControllerName, int totalPage, int pageSize, int currentPage, string sortColumn, string sortDirection)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            var str = new StringBuilder();

            var countTotal = (totalPage * 1.0) / pageSize;
            var totalPages = Convert.ToInt32(countTotal > 1 ? Math.Ceiling(countTotal) : countTotal);

            if (totalPages > 1)
            {
                str.Append("<ul class=\"pagination pagination-sm\">");
                int pageMin, pageMax;
                int prev, next;
                int first = 1;
                int last = totalPages;

                //low
                if (currentPage <= 3)
                    pageMin = 1;
                else
                    pageMin = (currentPage - 3);

                //this is previous
                if (currentPage == 1)
                {
                    prev = 0; // no need to show prev link
                    first = 0; //no need to show first link
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-fast-backward'></i></a>");
                    str.Append("</li>");
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-arrow-left'></i></ a>");
                }
                else
                {
                    prev = currentPage - 1;
                    str.Append("<li>");
                    str.Append(string.Format(htmlHelper.ActionLink("{0}", actionName, strControllerName, new { column = sortColumn, direction = sortDirection, page = first }, new { @class = "previous paginate_button" }).ToHtmlString(), "<i class='fa fa-fast-backward'></i>"));
                    str.Append("</li>");
                    str.Append("<li>");
                    str.Append(string.Format(htmlHelper.ActionLink("{0}", actionName, strControllerName, new { column = sortColumn, direction = sortDirection, page = prev }, new { @class = "previous paginate_button" }).ToHtmlString(), "<i class='fa fa-arrow-left'></i>"));
                    str.Append("</li>");
                }

                if ((totalPages - (pageMin + 6)) >= 0)
                {
                    pageMax = pageMin + 6;
                }
                else
                {
                    pageMax = totalPages;
                }

                for (int i = pageMin; i <= pageMax; i++)
                {
                    object objAttrib = null;
                    string active;
                    if (i == currentPage)
                    {
                        active = "active";
                        objAttrib = new { @class = "active" };
                    }
                    else
                    {
                        active = "paginate_button";
                        objAttrib = new { @class = "paginate_button" };
                    }
                    str.Append("<li class='" + active + "'>");
                    str.Append(htmlHelper.ActionLink(i.ToString(CultureInfo.InvariantCulture), actionName, strControllerName,
                                              new { column = sortColumn, direction = sortDirection, page = i }, objAttrib));
                    str.Append("</li>");
                }

                //this is next
                if (currentPage == totalPages)
                {
                    next = 0; //no need to show the next link
                    last = 0; //no need to show the last link
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-arrow-right'></i></a>");
                    //str.Append(htmlHelper.ActionLink(Displays.Common_Btn_Paging_Next, actionName, strControllerName,
                    //                         new { column = sortColumn, direction = sortDirection, page = next }, new { @class = "next paginate_button" }));
                    str.Append("</li>");
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-fast-forward'></i></a>");

                    //str.Append(htmlHelper.ActionLink(Displays.Common_Btn_Paging_Last, actionName, strControllerName,
                    //                         new { column = sortColumn, direction = sortDirection, page = last }, new { @class = "last paginate_button disabled" }));
                }
                else
                {
                    next = currentPage + 1;
                    str.Append("<li>");
                    str.Append(string.Format(htmlHelper.ActionLink("{0}", actionName, strControllerName, new { column = sortColumn, direction = sortDirection, page = next }, new { @class = "next paginate_button" }).ToHtmlString(), "<i class='fa fa-arrow-right'></i>"));
                    str.Append("</li>");
                    str.Append("</li>");
                    str.Append("<li>");
                    str.Append(string.Format(htmlHelper.ActionLink("{0}", actionName, strControllerName, new { column = sortColumn, direction = sortDirection, page = last }, new { @class = "last paginate_button" }).ToHtmlString(), "<i class='fa fa-fast-forward'></i>"));
                    str.Append("</li>");
                }
                str.Append("</ul>");
            }
            return new HtmlString(str.ToString());
        }

        public static HtmlString AjaxPagingHelper(this HtmlHelper htmlHelper, string actionAjax, int countRecord, int pageSize, int currentPage, string sortColumn = null, string sortDirection = null)
        {

            currentPage = (currentPage <= 0) ? 1 : currentPage;
            pageSize = (countRecord <= 0) ? SystemConfiguration.PageSizeDefault : pageSize;

            var str = new StringBuilder();
            var countTotal = (countRecord * 1.0) / pageSize;
            if (countTotal != 0)
            {
                var totalPages = Convert.ToInt32(countTotal > 1 ? Math.Ceiling(countTotal) : 1);

                if (totalPages > 0)
                {
                    str.Append("<div>");
                    var strShownumber = " từ " + ((currentPage - 1) * pageSize + 1) + " đến ";
                    if (countRecord > currentPage * pageSize)
                    {
                        strShownumber += (currentPage * pageSize).ToString();
                    }
                    else
                    {
                        strShownumber += countRecord.ToString();
                    }
                    str.Append("<div class=\"table-pagination d-flex-bw align-center\" style=\"justify-content: space-between !important;\">");
                    //str.Append("<div class=\"paging_show_number\" id=\"paging_show_number\" role=\"status\" aria-live=\"polite\">Hiển thị " + strShownumber + " trong " + countRecord + " bản ghi</div>");
                    str.Append("<div class=\"table_pagination__title\" id=\"paging_show_number\" role=\"status\" aria-live=\"polite\">Hiển thị " + strShownumber + " trong " + countRecord + " bản ghi</div>");
                    str.Append("<div class=\"table-pagination__control\">");
                    str.Append("<ul class=\"pagination custom_pagination\">");
                    int pageMin, pageMax;
                    int prev, next;
                    int first = 1;
                    int last = totalPages;

                    //low
                    if (currentPage <= 3)
                        pageMin = 1;
                    else
                        pageMin = (currentPage - 3);

                    //this is previous
                    if (currentPage == 1)
                    {
                        prev = 0; // no need to show prev link
                        first = 0; //no need to show first link
                        str.Append("<li class='disabled'>");
                        str.Append("<a href='javascript:void(0)' class='page-button'>" + Displays.Common_Btn_Paging_First + "</a>");
                        str.Append("</li>");
                        str.Append("<li class='disabled'>");
                        str.Append("<a class='paginate_button previous disabled page-button'>&lt;</a>");
                        str.Append("</li>");
                    }
                    else
                    {
                        prev = currentPage - 1;
                        str.Append("<li class='cursor-pointer'>");
                        str.Append("<a class = 'page-button first paginate_button'" +
                                   " onclick=\"" + actionAjax + "(" + first + ",'" + sortColumn + "','" + sortDirection + "')" + "\"> " + Displays.Common_Btn_Paging_First + "</i></a>");
                        str.Append("</li>");
                        str.Append("<li class='cursor-pointer'>");
                        str.Append("<a class = 'page-button previous paginate_button'" +
                                   " onclick=\"" + actionAjax + "(" + prev + ",'" + sortColumn + "','" + sortDirection + "')" + "\"><</a>");
                        str.Append("</li>");
                    }

                    if ((totalPages - (pageMin + 6)) >= 0)
                    {
                        pageMax = pageMin + 6;
                    }
                    else
                    {
                        pageMax = totalPages;
                    }

                    for (var i = pageMin; i <= pageMax; i++)
                    {
                        string objAttrib;
                        string active;
                        if (i == currentPage)
                        {
                            active = "active uk-active";
                            objAttrib = "class = 'paginate_active page-button'";
                            str.Append("<li class='cursor-pointer " + active + "'>");
                            str.Append("<a " + objAttrib + "\"> " + i + "</a>");
                        }
                        else
                        {
                            active = "paginate_button";
                            objAttrib = "class='paginate_button page-button'";
                            str.Append("<li class='cursor-pointer " + active + "'>");
                            str.Append("<a " + objAttrib + "  onclick=\"" + actionAjax + "(" + i + ",'" + sortColumn + "','" + sortDirection + "')" + "\"> " + i + "</a>");
                        }

                        str.Append("</li>");
                    }

                    //this is next
                    if (currentPage == totalPages)
                    {
                        next = 0; //no need to show the next link
                        last = 0; //no need to show the last link
                        str.Append("<li class='disabled'>");
                        str.Append("<a href='javascript:void(0)' class='paginate_button next page-button'>></a>");
                        str.Append("</li>");
                        str.Append("<li class='disabled'>");
                        str.Append("<a href='javascript:void(0)' class='paginate_button next page-button'>" + Displays.Common_Btn_Paging_Last + "</a></li>");
                    }
                    else
                    {
                        next = currentPage + 1;
                        str.Append("<li class='cursor-pointer'>");
                        str.Append("<a class = 'paginate_button next page-button' " +
                                   " onclick=\"" + actionAjax + "(" + next + ",'" + sortColumn + "','" + sortDirection + "')" + "\">></a>");
                        str.Append("</li>");
                        str.Append("</li>");
                        str.Append("<li class='cursor-pointer'>");
                        str.Append("<a class = 'last paginate_button page-button' " +
                                   " onclick=\"" + actionAjax + "(" + last + ",'" + sortColumn + "','" + sortDirection + "')" + "\">" + Displays.Common_Btn_Paging_Last + "</a>");
                        str.Append("</li>");
                    }
                    str.Append("</ul>");
                    str.Append("</div>");
                    str.Append("</div>");
                }
            }
            else
            {
                str.Append("<div>");
                str.Append("Không có bản ghi nào");
                str.Append("</div>");
            }

            return new HtmlString(str.ToString());
        }

        public static HtmlString PagingHelperForDishes(this HtmlHelper htmlHelper, int totalPage, int pageSize, int currentPage, string sortColumn, string sortDirection)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            var str = new StringBuilder();
            var totalPages = Convert.ToInt32(Math.Ceiling((totalPage * 1.0) / pageSize));
            if (totalPages > 1)
            {
                str.Append("<ul class='pagination pagination-sm'>");
                int pageMin, pageMax;
                int prev, next;
                int first = 1;
                int last = totalPages;

                //low
                if (currentPage <= 3)
                    pageMin = 1;
                else
                    pageMin = (currentPage - 3);

                //this is previous
                if (currentPage == 1)
                {
                    prev = 0; // no need to show prev link
                    first = 0; //no need to show first link
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-fast-backward'></i></a>");
                    str.Append("</li>");
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-arrow-left'></i></ a>");
                }
                else
                {
                    prev = currentPage - 1;
                    str.Append(string.Format("<li><a href='javascript:emenu.dishes.refreshGrid({0});' class='next paginate_button'><i class='fa fa-fast-backward'></i></a></li>", first));
                    str.Append(string.Format("<li><a href='javascript:emenu.dishes.refreshGrid({0});' class='next paginate_button'><i class='fa fa-arrow-left'></i></a></li>", prev));

                }

                if ((totalPages - (pageMin + 6)) >= 0)
                {
                    pageMax = pageMin + 6;
                }
                else
                {
                    pageMax = totalPages;
                }

                for (int i = pageMin; i <= pageMax; i++)
                {
                    string active;
                    if (i == currentPage)
                    {
                        active = "active";
                    }
                    else
                    {
                        active = "paginate_button";
                    }
                    str.Append(string.Format("<li class='" + active + "'><a href='javascript:emenu.dishes.refreshGrid({0});'>{1}</a>", i, i));
                }

                //this is next
                if (currentPage == totalPages)
                {
                    next = 0; //no need to show the next link
                    last = 0; //no need to show the last link
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-arrow-right'></i></a>");
                    str.Append("</li>");
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-fast-forward'></i></a>");
                }
                else
                {
                    next = currentPage + 1;
                    str.Append(string.Format("<li><a href='javascript:emenu.dishes.refreshGrid({0});' class='next paginate_button'><i class='fa fa-arrow-right'></i></a></li>", next));
                    str.Append(string.Format("<li><a href='javascript:emenu.dishes.refreshGrid({0});' class='next paginate_button'><i class='fa fa-fast-forward'></i></a>", last));
                }
                str.Append("</ul>");
            }
            return new HtmlString(str.ToString());
        }
        public static HtmlString PagingHelperForOrderLog(this HtmlHelper htmlHelper, int totalPage, int pageSize, int currentPage, string sortColumn, string sortDirection)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            var str = new StringBuilder();
            var totalPages = Convert.ToInt32(Math.Ceiling((totalPage * 1.0) / pageSize));
            if (totalPages > 1)
            {
                str.Append("<ul class='pagination pagination-sm'>");
                int pageMin, pageMax;
                int prev, next;
                int first = 1;
                int last = totalPages;

                //low
                if (currentPage <= 3)
                    pageMin = 1;
                else
                    pageMin = (currentPage - 3);

                //this is previous
                if (currentPage == 1)
                {
                    prev = 0; // no need to show prev link
                    first = 0; //no need to show first link
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-fast-backward'></i></a>");
                    str.Append("</li>");
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-arrow-left'></i></ a>");
                }
                else
                {
                    prev = currentPage - 1;
                    str.Append(string.Format("<li><a href='javascript:emenu.order.refreshGrid({0});' class='next paginate_button'><i class='fa fa-fast-backward'></i></a></li>", first));
                    str.Append(string.Format("<li><a href='javascript:emenu.order.refreshGrid({0});' class='next paginate_button'><i class='fa fa-arrow-left'></i></a></li>", prev));

                }

                if ((totalPages - (pageMin + 6)) >= 0)
                {
                    pageMax = pageMin + 6;
                }
                else
                {
                    pageMax = totalPages;
                }

                for (int i = pageMin; i <= pageMax; i++)
                {
                    string active;
                    if (i == currentPage)
                    {
                        active = "active";
                    }
                    else
                    {
                        active = "paginate_button";
                    }
                    str.Append(string.Format("<li class='" + active + "'><a href='javascript:emenu.order.refreshGrid({0});'>{1}</a>", i, i));
                }

                //this is next
                if (currentPage == totalPages)
                {
                    next = 0; //no need to show the next link
                    last = 0; //no need to show the last link
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-arrow-right'></i></a>");
                    str.Append("</li>");
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-fast-forward'></i></a>");
                }
                else
                {
                    next = currentPage + 1;
                    str.Append(string.Format("<li><a href='javascript:emenu.order.refreshGrid({0});' class='next paginate_button'><i class='fa fa-arrow-right'></i></a></li>", next));
                    str.Append(string.Format("<li><a href='javascript:emenu.order.refreshGrid({0});' class='next paginate_button'><i class='fa fa-fast-forward'></i></a>", last));
                }
                str.Append("</ul>");
            }
            return new HtmlString(str.ToString());
        }

        public static HtmlString PagingOrderLineItem(this HtmlHelper htmlHelper, string actionName, string strControllerName, int totalPage, int pageSize, int currentPage, string sortColumn, string sortDirection, int status)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            var str = new StringBuilder();
            var totalPages = Convert.ToInt32(Math.Ceiling((totalPage * 1.0) / pageSize));
            if (totalPages > 1)
            {
                str.Append("<ul class=\"pagination pagination-sm\">");
                int pageMin, pageMax;
                int prev, next;
                int first = 1;
                int last = totalPages;

                //low
                if (currentPage <= 3)
                    pageMin = 1;
                else
                    pageMin = (currentPage - 3);

                //this is previous
                if (currentPage == 1)
                {
                    prev = 0; // no need to show prev link
                    first = 0; //no need to show first link
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-fast-backward'></i></a>");
                    //str.Append(htmlHelper.ActionLink(Displays.Common_Btn_Paging_First, actionName, strControllerName,
                    //                         new { colunm = sortColumn, direction = sortDirection, page = first }, new { @class = "first paginate_button " }));
                    str.Append("</li>");
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-arrow-left'></i></ a>");
                    //str.Append(htmlHelper.ActionLink(Displays.Common_Btn_Paging_Previous, actionName, strControllerName,
                    //                                 new { column = sortColumn, direction = sortDirection, page = prev },
                    //                                 new { @class = "previous paginate_button" }));
                }
                else
                {
                    prev = currentPage - 1;
                    str.Append("<li>");
                    str.Append(string.Format(htmlHelper.ActionLink("{0}", actionName, strControllerName, new { column = sortColumn, direction = sortDirection, page = first, status = status }, new { @class = "previous paginate_button" }).ToHtmlString(), "<i class='fa fa-fast-backward'></i>"));
                    str.Append("</li>");
                    str.Append("<li>");
                    str.Append(string.Format(htmlHelper.ActionLink("{0}", actionName, strControllerName, new { column = sortColumn, direction = sortDirection, page = prev, status = status }, new { @class = "previous paginate_button" }).ToHtmlString(), "<i class='fa fa-arrow-left'></i>"));
                    str.Append("</li>");

                }

                if ((totalPages - (pageMin + 6)) >= 0)
                {
                    pageMax = pageMin + 6;
                }
                else
                {
                    pageMax = totalPages;
                }

                for (int i = pageMin; i <= pageMax; i++)
                {
                    object objAttrib = null;
                    string active;
                    if (i == currentPage)
                    {
                        active = "active";
                        objAttrib = new { @class = "active" };
                    }
                    else
                    {
                        active = "paginate_button";
                        objAttrib = new { @class = "paginate_button" };
                    }
                    str.Append("<li class='" + active + "'>");
                    str.Append(htmlHelper.ActionLink(i.ToString(CultureInfo.InvariantCulture), actionName, strControllerName,
                                              new { column = sortColumn, direction = sortDirection, page = i, status = status }, objAttrib));
                    str.Append("</li>");
                }

                //this is next
                if (currentPage == totalPages)
                {
                    next = 0; //no need to show the next link
                    last = 0; //no need to show the last link
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-arrow-right'></i></a>");
                    //str.Append(htmlHelper.ActionLink(Displays.Common_Btn_Paging_Next, actionName, strControllerName,
                    //                         new { column = sortColumn, direction = sortDirection, page = next }, new { @class = "next paginate_button" }));
                    str.Append("</li>");
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-fast-forward'></i></a>");

                    //str.Append(htmlHelper.ActionLink(Displays.Common_Btn_Paging_Last, actionName, strControllerName,
                    //                         new { column = sortColumn, direction = sortDirection, page = last }, new { @class = "last paginate_button disabled" }));
                }
                else
                {
                    next = currentPage + 1;
                    str.Append("<li>");
                    str.Append(string.Format(htmlHelper.ActionLink("{0}", actionName, strControllerName, new { column = sortColumn, direction = sortDirection, page = next, status = status }, new { @class = "next paginate_button" }).ToHtmlString(), "<i class='fa fa-arrow-right'></i>"));
                    str.Append("</li>");
                    str.Append("</li>");
                    str.Append("<li>");
                    str.Append(string.Format(htmlHelper.ActionLink("{0}", actionName, strControllerName, new { column = sortColumn, direction = sortDirection, page = last, status = status }, new { @class = "last paginate_button" }).ToHtmlString(), "<i class='fa fa-fast-forward'></i>"));
                    str.Append("</li>");
                }
                str.Append("</ul>");
            }
            return new HtmlString(str.ToString());
        }

        public static HtmlString AjaxPagingHelper(this HtmlHelper htmlHelper, string actionAjax, string action, string targetId, string targetData, int countRecord, int pageSize, int currentPage, string sortColumn, string sortDirection)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            var str = new StringBuilder();
            pageSize = (pageSize <= 0) ? 10 : pageSize;
            var strObj = "";
            if (!string.IsNullOrEmpty(targetData))
            {
                strObj = "," + '"' + targetData + '"';
            }
            var totalPages = Convert.ToInt32(Math.Ceiling((countRecord * 1.0) / pageSize));
            if (totalPages > 0)
            {
                str.Append("<ul class=\"pagination pagination-sm\">");
                int pageMin, pageMax;
                int prev, next;
                int first = 1;
                int last = totalPages;

                //low
                if (currentPage <= 3)
                    pageMin = 1;
                else
                    pageMin = (currentPage - 3);

                //this is previous
                if (currentPage == 1)
                {
                    prev = 0; // no need to show prev link
                    first = 0; //no need to show first link
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-fast-backward'></i></a>");
                    str.Append("</li>");
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-arrow-left'></i></ a>");
                }
                else
                {
                    prev = currentPage - 1;
                    str.Append("<li class='cursor-pointer'>");
                    str.Append("<a class = 'first paginate_button'" +
                               " onclick='" + actionAjax + "(" + first + ',' + '"' + action + '"' + ',' + pageSize + ',' + '"' + targetId + '"' + strObj + ")'> <i class='fa fa-fast-backward'></i></a>");
                    str.Append("</li>");
                    str.Append("<li class='cursor-pointer'>");
                    str.Append("<a class = 'previous paginate_button'" +
                               "onclick='" + actionAjax + "(" + prev + ',' + '"' + action + '"' + ',' + pageSize + ',' + '"' + targetId + '"' + strObj + ")'> <i class='fa fa-arrow-left'></i></a>");
                    str.Append("</li>");
                }

                if ((totalPages - (pageMin + 6)) >= 0)
                {
                    pageMax = pageMin + 6;
                }
                else
                {
                    pageMax = totalPages;
                }

                for (int i = pageMin; i <= pageMax; i++)
                {
                    string objAttrib;
                    string active;
                    if (i == currentPage)
                    {
                        active = "active";
                        objAttrib = "class = 'paginate_active'";
                    }
                    else
                    {
                        active = "paginate_button";
                        objAttrib = "class='paginate_button'";
                    }

                    str.Append("<li class='cursor-pointer " + active + "'>");
                    str.Append("<a " + objAttrib + " onclick='" + actionAjax + "(" + i + ',' + '"' + action + '"' + ',' + pageSize + ',' + '"' + targetId + '"' + strObj + ")'> " + i + "</a>");

                    str.Append("</li>");
                }

                //this is next
                if (currentPage == totalPages)
                {
                    next = 0; //no need to show the next link
                    last = 0; //no need to show the last link
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-arrow-right'></i></a>");
                    str.Append("</li>");
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-fast-forward'></i></a>");
                }
                else
                {
                    next = currentPage + 1;
                    str.Append("<li class='cursor-pointer'>");
                    str.Append("<a class = 'next paginate_button' " +
                               " onclick='" + actionAjax + "(" + next + ',' + '"' + action + '"' + ',' + pageSize + ',' + '"' + targetId + '"' + strObj + ")'> <i class='fa fa-arrow-right'></i></a>");
                    str.Append("</li>");
                    str.Append("</li>");
                    str.Append("<li class='cursor-pointer'>");
                    str.Append("<a class = 'last paginate_button' " +
                               " onclick='" + actionAjax + "(" + last + ',' + '"' + action + '"' + ',' + pageSize + ',' + '"' + targetId + '"' + strObj + ")'><i class='fa fa-fast-forward'></i></a>");
                    str.Append("</li>");
                }
                str.Append("</ul>");
            }
            return new HtmlString(str.ToString());
        }

        public static HtmlString PagingSearchHelper(this HtmlHelper htmlHelper, string actionName, string strControllerName, int totalPage, int pageSize, int currentPage, string sortColumn, string sortDirection, string name, string languageId, string categoryId)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            var str = new StringBuilder();
            var totalPages = Convert.ToInt32(Math.Ceiling((totalPage * 1.0) / pageSize));
            if (totalPages > 1)
            {
                str.Append("<ul class=\"pagination pagination-sm\">");
                int pageMin, pageMax;
                int prev, next;
                int first = 1;
                int last = totalPages;

                //low
                if (currentPage <= 3)
                    pageMin = 1;
                else
                    pageMin = (currentPage - 3);

                //this is previous
                if (currentPage == 1)
                {
                    prev = 0; // no need to show prev link
                    first = 0; //no need to show first link
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-fast-backward'></i></a>");

                    str.Append("</li>");
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-arrow-left'></i></ a>");
                }
                else
                {
                    prev = currentPage - 1;
                    str.Append("<li>");
                    str.Append(string.Format(htmlHelper.ActionLink("{0}", actionName, strControllerName, new { column = sortColumn, direction = sortDirection, name, languageId, categoryId, page = first }, new { @class = "previous paginate_button" }).ToHtmlString(), "<i class='fa fa-fast-backward'></i>"));
                    str.Append("</li>");
                    str.Append("<li>");
                    str.Append(string.Format(htmlHelper.ActionLink("{0}", actionName, strControllerName, new { column = sortColumn, direction = sortDirection, name, languageId, categoryId, page = prev }, new { @class = "previous paginate_button" }).ToHtmlString(), "<i class='fa fa-arrow-left'></i>"));
                    str.Append("</li>");

                }

                if ((totalPages - (pageMin + 6)) >= 0)
                {
                    pageMax = pageMin + 6;
                }
                else
                {
                    pageMax = totalPages;
                }

                for (int i = pageMin; i <= pageMax; i++)
                {
                    object objAttrib = null;
                    string active;
                    if (i == currentPage)
                    {
                        active = "active";
                        objAttrib = new { @class = "active" };
                    }
                    else
                    {
                        active = "paginate_button";
                        objAttrib = new { @class = "paginate_button" };
                    }
                    str.Append("<li class='" + active + "'>");
                    str.Append(htmlHelper.ActionLink(i.ToString(CultureInfo.InvariantCulture), actionName, strControllerName,
                                              new { column = sortColumn, direction = sortDirection, name, languageId, categoryId, page = i }, objAttrib));
                    str.Append("</li>");
                }

                //this is next
                if (currentPage == totalPages)
                {
                    next = 0; //no need to show the next link
                    last = 0; //no need to show the last link
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-arrow-right'></i></a>");
                    str.Append(htmlHelper.ActionLink("Trang sau", actionName, strControllerName,
                                              new { column = sortColumn, direction = sortDirection, name, languageId, categoryId, page = next }, new { @class = "next paginate_button" }));

                    str.Append("</li>");
                    str.Append("<li class='disabled'>");
                    str.Append("<a href='javascript:void(0)'><i class='fa fa-fast-forward'></i></a>");

                    str.Append(htmlHelper.ActionLink("Trang cuối", actionName, strControllerName,
                                             new { column = sortColumn, direction = sortDirection, name, languageId, categoryId, page = last }, new { @class = "last paginate_button disabled" }));
                }
                else
                {
                    next = currentPage + 1;
                    str.Append("<li>");
                    str.Append(string.Format(htmlHelper.ActionLink("{0}", actionName, strControllerName, new { column = sortColumn, direction = sortDirection, name, languageId, categoryId, page = next }, new { @class = "next paginate_button" }).ToHtmlString(), "<i class='fa fa-arrow-right'></i>"));
                    str.Append("</li>");
                    str.Append("</li>");
                    str.Append("<li>");
                    str.Append(string.Format(htmlHelper.ActionLink("{0}", actionName, strControllerName, new { column = sortColumn, direction = sortDirection, name, languageId, categoryId, page = last }, new { @class = "last paginate_button" }).ToHtmlString(), "<i class='fa fa-fast-forward'></i>"));
                    str.Append("</li>");
                }
                str.Append("</ul>");
            }
            return new HtmlString(str.ToString());
        }

        public static string FormatDate(this HtmlHelper htmlHelper, DateTime dateTime)
        {
            return dateTime.ToString("dd-MM-yyyy hh:mm");
        }

        public static string FormatMoney(this HtmlHelper htmlHelper, decimal? money)
        {
            if (money.HasValue && money != 0)
            {
                var cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"                                                                 // return money.Value.ToString("#,###", cul.NumberFormat);
                return string.Format(cul, "{0:#,0.##}", money.Value);
            }

            return "0";
        }

        //public static string FormatMoney(this HtmlHelper htmlHelper, decimal money, string currency = "VND")
        //{
        //    if (currency == "VND")
        //    {
        //        var cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
        //        return money.ToString("#,###", cul.NumberFormat);
        //    }
        //    else
        //    {
        //        var cul = CultureInfo.GetCultureInfo("en-US");   // try with "en-US"
        //        return money.ToString("#,###", cul.NumberFormat);
        //    }
        //}
        public static string FormatMoney2(this HtmlHelper htmlHelper, decimal? money)
        {
            if (money.HasValue && money.Value != 0)
            {
                return money.Value.ToString("#.##");
            }

            return "0";
        }

        public static string MapStatusOrderToString(this HtmlHelper htmlHelper, int status)
        {
            var strStatus = "";
            switch (status)
            {
                case 0:
                case 1:
                    strStatus = "Chờ xử lý";
                    break;
                case 2:
                    strStatus = "Chờ báo giá";
                    break;
                case 3:
                    strStatus = "Đã báo giá";
                    break;
                case 4:
                    strStatus = "Chờ đặt cọc";
                    break;
                case 5:
                    strStatus = "Chờ mua hàng";
                    break;
                case 6:
                case 7:
                    strStatus = "Đã mua hàng";
                    break;
                case 8:
                case 9:
                    strStatus = "Nhập kho TQ";
                    break;
                case 10:
                case 11:
                    strStatus = "Nhập kho VN";
                    break;
                case 12:
                    strStatus = "Đã hoàn thành";
                    break;
            }

            return strStatus;
        }
        public static string GetDisplayName(this HtmlHelper htmlHelper, Enum enumValue)
        {
            return enumValue.GetType()?
           .GetMember(enumValue.ToString())?[0]?
           .GetCustomAttribute<DisplayAttribute>()?
           .Name;
        }
        public static MvcHtmlString File(this HtmlHelper helper, string fullName, DateTime dateNow)
        {
            var rootPath = string.Concat("http://" + HttpContext.Current.Request.Url.Authority, "/App_Data/ExcelReport/Taxes");
            var formatDate = DateTime.Now.Day + "_" + dateNow.Month + "_" + dateNow.Year;
            var folderPath = string.Concat(rootPath, "\\" + formatDate);

            var result = string.Concat(folderPath, "\\" + fullName + "_" + formatDate + ".docx");
            return MvcHtmlString.Create(result);
        }


    }
}