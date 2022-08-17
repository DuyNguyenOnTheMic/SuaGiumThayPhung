//require jquery
(function (global) {
    if (!global) {
        throw "Global Window object is not available.";
    }
    var Capstone = function () {
        return new Capstone.init();
    }

    function _base64ToArrayBuffer(base64) {
        var binary_string = window.atob(base64);
        var len = binary_string.length;
        var bytes = new Uint8Array(len);
        for (var i = 0; i < len; i++) {
            bytes[i] = binary_string.charCodeAt(i);
        }
        return bytes.buffer;
    }

    function saveByteArray(reportName, byte) {
        var blob = new Blob([byte], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
        var isIE = false || !!document.documentMode;
        if (isIE) {
            window.navigator.msSaveBlob(blob, reportName);
        } else {
            var url = window.URL || window.webkitURL;
            link = url.createObjectURL(blob);
            var a = $("<a />");
            a.attr("download", reportName);
            a.attr("href", link);
            $("body").append(a);
            a[0].click();
            $("body").remove(a);
        }
    };

    Capstone.init = function () { }
    Capstone.prototype = {
		dataTable: function (className, pdfUrl) {
			console.log('.' + className);
			setTimeout(function (){
				$('.' + className).DataTable({
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
					buttons: [
						{
							extend: 'copy',
							charset: 'utf-8',
							fieldBoundary: '',
							bom: true,
							exportOptions: {
								columns: ['.export']
							}
						}, {
							extend: 'csv',
							charset: 'utf-8',
							extension: '.csv',
							fieldBoundary: '',
							bom: true,
							exportOptions: {
								columns: ['.export']
							}
						},
						{
							extend: 'pdf',
							charset: 'utf-8',
							extension: '.pdf',
							fieldBoundary: '',
							bom: true,
							exportOptions: {
								columns: ['.export']
							}
						},
						//{
						//	text: 'PDF*',
						//	action: function (e, dt, node, config) {
						//		alert(pdfUrl);
						//	}
						//},
						{
							extend: 'print',
							charset: 'utf-8',
							fieldBoundary: '',
							bom: true,
							exportOptions: {
								columns: ['.export']
							}
						}
					]
				});
			}, 0);
			
        },

        exportFile: function (message, fileName) {
            var bytes = _base64ToArrayBuffer(message);
            saveByteArray(fileName, bytes);
        }
    };
    Capstone.init.prototype = Capstone.prototype;
    global.Capstone = global.$C = Capstone;
}(window));
