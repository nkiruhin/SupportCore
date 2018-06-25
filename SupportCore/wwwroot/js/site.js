// Write your JavaScript code.
//$('.jselect2').select2({
//    //ajax: {
//    //    url: function (params) {
//    //        console.dir(params);
//    //        console.log($('.jselect2').data('ajax-Url'));
//    //        return (document.URL + $('.jselect2').data('ajax-Url'))
//    //    },
//    //    dataType: 'json'
//    // Additional AJAX parameters go here; see the end of this chapter for the full code of this example
//    // }

//});
var signalRStart = function () {
    console.log("signalr client start")
    const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub")
    .build();
    connection.on("ReceiveMessage", (user,date, message) => {
    //console.log("Принято сообщение:" + message + "от "+user+"в "+date)
        Metro.notify.create(message, 'Оповещение от ' + user + ' ' + date, { cls: "info", width: 300 });
    });
    connection.start().catch(err => console.error(err.toString()));
}
function TableReady(url, columns) {
    var Table = $('.dataTable').DataTable({
        "searching": true,
        "ordering": true,
        "order": [],
        //"serverSide": true,
        "processing": true,
        //"scrollY": "200px",
        //"scrollCollapse": true,
        "ajax": {
            "url": url
            //"url": "/Clients/ViewTableJson", "dataSrc": "clients", "type": "POST",
            //"data": function (d) {
            //    d.FirstName = $('#FirstName').val(),
            //        d.LastName = $('#LastName').val(),
            //        d.PatrName = $('#PatrName').val(),
            //        d.BirthDate = $('#BirthDate').val()
            //    d.recordsTotal = $('#recordsTotal').val()
            //}
        },
        "language": {
            "lengthMenu": "Отображать _MENU_ записей",
            "zeroRecords": "Не найдено",
            "info": "Показано с _START_ по _END_ из _TOTAL_ записей",
            "infoEmpty": "Нет данных",
            "search": "Фильтр:",
            "processing": "<div data-role='activity' data-type='metro' data-style='dark'></div>",
            "paginate": {
                "first": "Первый",
                "last": "Последний",
                "next": "Следующий",
                "previous": "Предыдущий"
            },
        },
        //"columnDefs": [{
        //    "targets": -1,
        //    "data": 'id',
        //    "render": function (data, type, full, meta) {
        //        return '<a href="#" data-id=' + data + ' id="Details"><span class="mif-profile"></span></a> | <a href="#" data-id=' + data + ' id="Edit" class="Edit" ><span class="mif-pencil"></span></a>'
        //    }
        //}

        //],
        "columns": columns // [
        //  { "data": "name" },
        //  { "data": "email" },
        //    { "data": "patrName" },
        //    { "data": "birthDate" },
        //    { "data": "sex" },
        //    { "data": "snils" },
        //    { "data": "clientpolicyNumber" }
        // ]
    })
    return Table;
}
function SimpleTableReady() {
    var Table = $('.dataTable').DataTable({
        "searching": true,
        "ordering": true,
        //"serverSide": true,
        "processing": true,
        "order": [[0, 'desc']],
        //"scrollY": "200px",
        //"scrollCollapse": true,
        "language": {
            "lengthMenu": "Отображать _MENU_ записей",
            "zeroRecords": "Не найдено",
            "info": "Показано с _START_ по _END_ из _TOTAL_ записей",
            "infoEmpty": "Нет данных",
            "search": "Фильтр:",
            "processing": "Обрабатываю...",
            "paginate": {
                "first": "Первый",
                "last": "Последний",
                "next": "Следующий",
                "previous": "Предидущий"
            },
        },
    })
    return Table;
}
//Editable table
function edit(element) {
    var tr = jQuery(element).parent().parent();
    if (!tr.hasClass("editing")) {
        tr.addClass("editing");
        tr.find("DIV.td").each(function () {
            if (!jQuery(this).hasClass("action")) {
                var value = jQuery(this).text();
                jQuery(this).text("");
                jQuery(this).append('<input type="text" value="' + value + '" />');
            } else {
                jQuery(this).find("BUTTON").text("save");
            }
        });
    } else {
        tr.removeClass("editing");
        tr.find("DIV.td").each(function () {
            if (!jQuery(this).hasClass("action")) {
                var value = jQuery(this).find("INPUT").val();
                jQuery(this).text(value);
                jQuery(this).find("INPUT").remove();
            } else {
                jQuery(this).find("BUTTON").text("edit");
            }
        });
    }
}

//CustomDialogWin
function openCustomWin(href, icon, title, modal) {
    $.get(href, function (html) {
      Metro.window.create({
          title: title,
          overlay:true,
          shadow:true,
          modal: modal,
          overlayColor: "#000000",
          clsCaption: "bg-darkCyan",
          icon: "<span class='" + icon + "'></span>",
          content: html,
          place: "center"
        });
    })
}
//Close Win
var closeWin = function () {
    $('.window').next().remove();
    $('.overlay').remove();
    $('.window').remove();       
}
//CustomDialog
function openCustomDialog(href, title, end) {
    $.get(href, function (html) {
        Metro.dialog.create({
            title: title+'?',
            content: html,
            actions: [
                {
                    caption: title,
                    cls: "js-dialog-close alert",
                    onclick: function () {
                        end();
                    }
                },
                {
                    caption: "Отмена",
                    cls: "js-dialog-close"
                }
            ]
        });
    });
}
//Save Notify
var onSuccess = function (context) {
    
        Metro.notify.create(context, "Удачно", { cls: "success"});
};
//Error Notify
var onError = function (context) {
        Metro.notify.create(context, "Ошибка", { cls: "alert" });
};
// Select organization email
function orgEmail() {
    var check = document.getElementById("EmailOrg").checked;
    if (check === true) {
        if ($('.jselect2').find(':selected').length === 0) {
            onError("Организация не выбрана");
            document.getElementById("EmailOrg").checked = false;
            return;
        }
        $("#Email").val($('.jselect2').find(':selected').text().match(/<(.+)>/i)[1]);
        console.log($('.jselect2').find(':selected').text().match(/<(.+)>/i)[1]);
    }
}
//Load counters
function getCounters() {
    var url = 'Tickets/GetTicketCounters';
    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            var counters = JSON.parse(this.responseText)
            for (i in counters) {
                document.getElementById(i).innerText = counters[i]
            }
        }
    };
    // 2. Конфигурируем его: GET-запрос на URL 
    xhr.open('GET', url, true);
    xhr.send();
}
//Create Ticket with ajax
function ticketCreate(url) {
    var form = document.forms.TicketCreate;
    form.addEventListener('submit', function (ev) {
        console.log($('#TicketCreate').valid());
        if (!$('#TicketCreate').valid()) return false;
        var formData = new FormData(form);
        document.getElementById("tCreateSubmit").disabled = 'disabled';
        var preloader = document.getElementById('createPreloader');
        preloader.style.display = 'block';
        var xhr = new XMLHttpRequest();  
        xhr.open("POST", url, true);
        xhr.onreadystatechange = function () {
            if (this.readyState === 4 && this.status === 200) {
                if (this.status === 200) {
                    $('#cell-content').html(this.responseText);
                    getCounters();
                }
                else {
                    onError('Ошибка сохранения')
                    document.getElementById("tCreateSubmit").disabled = 'enabled';
                    preloader.style.display = 'none';
                };
            } else if (this.readyState === 4) {
                onError('Ошибка сохранения:' + this.status);
                document.getElementById("tCreateSubmit").disabled = 'enabled';
                preloader.style.display = 'none';
            }
        };
        xhr.send(formData);
        ev.preventDefault();
        return false;
    });
}
//Save Ticket with ajax
function ticketSave(url) {
    var form = document.forms.EditTicket;
    var formData = new FormData(form);
    var xhr = new XMLHttpRequest();
    var preloader = document.getElementById("editPreloader")
    preloader.style.display = "block";
    xhr.open("POST", url, true);
    xhr.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) { $('#cell-content').html(this.responseText) }
            else { onError('Ошибка сохранения') };
        }
    };
    preloader.style.display = "none";
    xhr.send(formData);
    getCounters();
}
//Submit for end form
var end = function () {
    $('#end').submit();
}
var loadTicketStat = function (url) {
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccessChart
    });
}
var OnSuccessChart = function (response) {
    var result = response.data;
    var arrayLabels = [], arraySeries = [], arraySeria = [];
    //console.log(response);
    $.map(result, function (item, index) {

        arrayLabels.push(item.labels); arraySeries.push(item.summ);

        //arraySeria.push(item.summ);
    });             //for Bar

    //arraySeries.push(arraySeria);
    var data = {
        labels: arrayLabels,
        series: arraySeries
    }
    var options = {
        labelInterpolationFnc: function (value) {
            return value[0]
        }
    };

    var responsiveOptions = [
        ['screen and (min-width: 640px)', {
            chartPadding: 30,
            labelOffset: 100,
            labelDirection: 'explode',
            labelInterpolationFnc: function (value) {
                return value;
            }
        }],
        ['screen and (min-width: 1024px)', {
            labelOffset: 37,
            chartPadding: 20,
            labelDirection: 'explode',
            labelInterpolationFnc: function (value) {
                return value;
            }
        }]
    ];
    if (response.type === 'C') new Chartist.Pie('#catChart', data, options, responsiveOptions);
    if (response.type === 'S') new Chartist.Pie('#statusChart', data, options, responsiveOptions);
}
var loadTicketStatforStaff = function (url) {
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccessChartforStaff
    });
}
var OnSuccessChartforStaff = function (response) {
    var data = response;
    var options = {
        seriesBarDistance: 10
    };

    var responsiveOptions = [
        ['screen and (max-width: 640px)', {
            seriesBarDistance: 5,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value[0];
                }
            }
        }]
    ];

    new Chartist.Bar('#catChart', data, options, responsiveOptions);
};
$(document).ajaxError(function (e, xhr) {
    if (xhr.status === 401)
        window.location = "/Account/Login";
    else if (xhr.status === 403)
        alert("You have no enough permissions to request this resource.");
});


