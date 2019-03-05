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
var sipClientStart = function (configuration) {
    phone = new SIP.UA(configuration);
    phone.start();
    phone.on('disconnected', function (e) {
        console.log("Ошибка подключения")
        $('.registred').toggleClass('mif-phonelink-off')
        phone.stop();
    });
    phone.on('registrationFailed', function (e) {
        console.log("Ошибка регистрации")
        configuration.uri = null;
        configuration.password = null;
        $('.registred').toggleClass('mif-phonelink-off')
        phone.stop();
    });
    phone.on('registered', function (e) {
        $('.registred').toggleClass('mif-phonelink');
    });
    phone.on('invite', function (session) {
        console.log('incoming');
        console.log(session.remoteIdentity.displayName);
        console.log(session.remoteIdentity.uri.user);
        var display_name = session.remoteIdentity.displayName || session.remoteIdentity.uri.user;
        var url = '@Url.Action("GetPersonForPhone", "Person")/' + display_name;
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            if (this.readyState === 4 && this.status === 200) {
                console.log(this.responseText);
                var notify = Metro.notify;
                notify.create(this.responseText, 'Звонок в ' + new Date().toLocaleTimeString('ru-RU'), {
                    keepOpen: true,
                    width: 'auto',
                });
            }
        };
        // 2. Конфигурируем его: GET-запрос на URL 'phones.json'
        xhr.open('GET', url, true);
        xhr.send();
    });
};
var signalRStart = function () {
    console.log("signalr client start");
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/hub")
        .build();
    connection.on("ReceiveMessage", (user, date, message, isuser) => {
        //console.log("Принято сообщение:" + message + " от " + user + " в " + date + "isuser" + isuser);
        if (!isuser) {
            Metro.notify.create(message, 'Оповещение от ' + user + ' ' + date, { cls: "info", width: 300 });
        } else {
            //addEventCounter();
            Metro.notify.create(message, 'Оповещение от ' + user + ' ' + date, { cls: "alert", width: 300 });
            addEvent(message,user,date);
        }
    });
    connection.start().catch(err => console.error(err.toString()));
};
function TableReady(url, columns) {
    var Table = $('.dataTable').DataTable({
        "searching": true,
        "ordering": true,
        "order": [],
        //"serverSide": true,
        "processing": true,
        "responsive": true,
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
            "infoFiltered": "(отфильтровано из _MAX_ записей)",
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
    });
    return Table;
}
function SimpleTableReady() {
    var Table = $('.dataTable').DataTable({
        "searching": true,
        "ordering": true,
        //"serverSide": true,
        "processing": true,
        "order": [[0, 'desc']],
        "responsive": true,
        //"scrollY": "200px",
        //"scrollCollapse": true,
        "language": {
            "lengthMenu": "Отображать _MENU_ записей",
            "zeroRecords": "Не найдено",
            "info": "Показано с _START_ по _END_ из _TOTAL_ записей",
            "infoFiltered": "(отфильтровано из _MAX_ записей)",
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
function openCustomWin(href, icon, title, modal, width) {
    if (!width) width = 'auto';
    console.log(width);
    $.get(href, function (html) {
            Metro.window.create({
            title: title,
            overlay: true,
            shadow: true,
            modal: modal,
            overlayColor: "#000000",
            clsCaption: "bg-darkCyan",
            width: width,
            icon: "<span class='" + icon + "'></span>",
            content: html,
            place: "center"
        });
    });
}
//Close Win
var closeWin = function () {
    $('.window').next().remove();
    $('.overlay').remove();
    $('.window').remove();
};
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
                    cls: "js-dialog-close secondary"
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
//Load counters in Home/SideBar
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
//Create Ticket with ajax Ticket/Create
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
                    onError('Ошибка сохранения');
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
//Save Ticket with ajax Ticket/Edit
function ticketSave(url) {
    var form = document.forms.EditTicket;
    var formData = new FormData(form);
    var xhr = new XMLHttpRequest();
    var preloader = document.getElementById("editPreloader");
    preloader.style.display = "block";
    xhr.open("POST", url, true);
    xhr.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) { $('#cell-content').html(this.responseText); }
            else { onError('Ошибка сохранения'); }
        }
    };
    preloader.style.display = "none";
    xhr.send(formData);
    getCounters();
}
//Submit for end event in form
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
};
var loadTicketStatforStaff = function (url) {
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccessChartforStaff
    });
};
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

function closed(id, table) {
    $.get('/TicketThreads/IsTask?Id=' + id, function (data) {
        if (data === 'true') {
            Metro.infobox.create("<h6>Действие невозможно!</h6><p>В заявке имеются не закрытые задачи!", "alert");
            return;
        }
        table_p = '';
        if (table) table_p = '&table=true';
        openCustomWin('/TicketThreads/Create/' + id + '?Type=1&Event=2' + table_p, 'mif-pencil', 'Закрытие', true);
    });
}

$(document).ajaxError(function (e, xhr) {
    if (xhr.status === 401)
        window.location = "/Account/Login";
    else if (xhr.status === 403)
        alert("You have no enough permissions to request this resource.");
});

// For ajax data return
// Get FildValue for Slays
var GetFieldValue = function (url, field) {
    var option = '';
    $.getJSON(url, function (data) {
        $.each(data, function (i, value) {
            option += '<option value="' + value + '">' + value + '</option>';
        });
        field.html(option);
    });
};
var enableSelect = function () {  // Enable select2 on ticket forms for nonUser
     var personSelect = $('#PersonId').select2({
        placeholder: 'Выберите пользователя',
        language: 'ru'
    });
    personSelect.data('select2').$container.addClass("wbutton");

    $('#StaffId').select2({
        placeholder: 'Выберите сотрудника',
        language: 'ru'
    });
};
//Enabled and disabled custom input it Ticket/Create and Edit for user 
var disInput = function () {
    $('#StaffId').prop('disabled', true);
    $('#PersonId').prop('disabled', true);
    $('#SourceId').prop('disabled', true);
    $('#SourceId').css('color', 'black');
    $('.inbutton').hide();
    $('#DueDate').removeAttr('data-role');
    $('#DueDate').prop('readonly', true);
    $('#DueDate').css('background-color', '#e9e9e9');
    $('.checkbox').hide();
};
var addEvent = function (message,user,date) {
    var bell = $('.eventAlarm');
    if (!$(bell).hasClass('mif-bell')) {
        $(bell).toggleClass('mif-bell ani-ring mif-lg');
        $(bell).after('<span class="badge inside bg-red fg-white" id="eventCount">1</span>');
    } else {       
        $('#eventCount').html(+$('#eventCount').text() + 1);
    }
    var events = '<blockquote class="place-right" style="border-left-color: #13709e"><p>' + message + '<p><small>' + 'От ' + user + ' ' + date + '</small></blockquote>' + $(bell).data('popoverText');
    $(bell).data('popoverText', events).attr('data-popover-text', events);
};
//Load response template for TicketThread 
var getTemplate = function (editor, url) {
    var TemplateId = document.getElementById("TemplateId").value;
    url = url + TemplateId;
    //console.log(url);
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            if (this.responseText === 'empty') {
                $('#Body').trumbowyg(this.responseText);
            } else {
                $('#Body').trumbowyg('html', this.responseText);
            }
        }
    };
    xhttp.open("GET", url, true);
    xhttp.send();
};