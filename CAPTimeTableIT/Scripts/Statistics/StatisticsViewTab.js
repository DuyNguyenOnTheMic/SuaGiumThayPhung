{
    https://cdnjs.cloudflare.com/ajax/libs/mustache.js/2.3.0/mustache.min.js
        <><script id="my-template" type="x-tmpl-mustache">
        <table id='teacherStatisticTable' class="table hover multiple-select-row data-table-export nowrap" style="background-color:white">
            <thead>
                <tr style="background-color: #222659; color: white">
                    <th>Giảng viên</th>
                    <th>Giờ dạy</th>
                </tr>
            </thead>
            <tbody>
                {{ #data: }}
                <tr>
                    <td>{{ fullName }}</td>
                    <td>{{ totalHours }}</td>
                </tr>
                {{} / data}{"}"}
            </tbody>
        </table>

    </script><script type="text/javascript">
            window.addEventListener('DOMContentLoaded', function ()
            var image = document.getElementById('image');
            var cropBoxData;
            var canvasData;
            var cropper;

            $('#modal').on('shown.bs.modal', function () {cropper = new Cropper(image, {
                autoCropArea: 0.5,
                dragMode: 'move',
                aspectRatio: 3 / 3,
                restore: false,
                guides: false,
                center: false,
                highlight: false,
                cropBoxMovable: false,
                cropBoxResizable: false,
                toggleDragModeOnDblclick: false,
                ready: function() {
                    cropper.setCropBoxData(cropBoxData).setCanvasData(canvasData);
                }
            })};
            {"}"}).on('hidden.bs.modal', function () {cropBoxData = cropper.getCropBoxData()};
            canvasData = cropper.getCanvasData();
            cropper.destroy();
            {"}"});
            {"}"});
            $(document).ready(function ()
            var semesterId = @latestSemesterId;
            var dataPromise = getData(semesterId);
            dataPromise.then(function (teacherHoursDataResponse) {renderBarChart(teacherHoursDataResponse)};
            var template = $('#my-template').html();
            //console.log(template);
            var output = $('#tableStatistic');
            var result = Mustache.render(template, teacherHoursDataResponse);
            //console.log(teacherHoursDataResponse.data);
            output.append(result);
            //render tables
            setTimeout(function () {$('#teacherStatisticTable').DataTable({
                scrollCollapse: true,
                autoWidth: false,
                responsive: true,
                columnDefs: [{
                    targets: "datatable-nosort",
                    orderable: false,
                }],
                "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                "language": {
                    "info": "_START_-_END_ of _TOTAL_ entries",
                    searchPlaceholder: "Search",
                    paginate: {
                        next: '<i class="ion-chevron-right"></i>',
                        previous: '<i class="ion-chevron-left"></i>'
                    }
                },
                dom: 'Bfrtp',
                buttons: []
            })};
            {"}"}, 0);

            //todo apply simple table
            {"}"});
            {"}"});

            function getData(semesterId)
            var promise = $.ajax({url}: "@Url.Content("~/Statistics/GetTeacherHourBySemesterId/")" + semesterId,
            method: 'POST'
            {"}"}).done(function (response) );
            return promise;
            {"}"}

            function renderBarChart(response) {
                //render barchart
            }
        //render barchart

            var data = response.data;
            // Initialize the echarts instance based on the prepared dom
            var myChart = echarts.init(document.getElementById('main'));

            var dataForChart = [];
            for (var i = 0; i <data.length /> i++)
            var row = [];
            row.push(data[i].userId);
            row.push(data[i].fullName);
            row.push(data[i].totalHours);
            dataForChart.push(row);
            {"}"}
            // Specify the configuration items and data for the chart
            var option = {dataset}: [
            {dimensions}: ['userId', 'fullName', 'totalHours'],
            source: dataForChart
            {"}"},
            {transform}: {type}: 'sort',
            config: {dimension}: 'totalHours', order: 'asc' {"}"}
            {"}"}
            {"}"}
            ],
            yAxis: {type}: 'category',
        @*axisLabel: {interval}: 0, rotate: 0 {"}"}*@
            {"}"},
            xAxis: ,
            series: {type}: 'bar',
            encode: {x}: 'totalHours', y: 'fullName' {"}"},
            datasetIndex: 1,
            backgroundStyle: {color}: 'rgba(201, 203, 207, 0.2)'
            {"}"},
            itemStyle: {color}: 'rgba(54, 162, 235, 0.2)'
            {"}"},
            label: {show}: true,
            position: 'right'
            {"}"}
            {"}"}
            {"}"};

            // Display the chart using the configuration items and data just specified.
            myChart.setOption(option);
            {"}"}
        </script></></>
}