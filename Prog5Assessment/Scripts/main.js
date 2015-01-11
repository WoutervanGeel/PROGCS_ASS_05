$(function () {

    $(".datepicker").each(function () {
        var dateinput = $(this).val();
        if (dateinput.length > 0) {
            var correctDate = dateConvert(dateinput);
            $(this).datetimepicker({
                value: correctDate,
                formatTime: 'H:i',
                formatDate: 'Y-m-d',
                step: 5,
                inline: true,
                onChangeDateTime: function (dp, $input) {
                    $(this).val($input.val());
                }
            });
        }
        else {
            $(this).datetimepicker({
                formatTime: 'H:i',
                formatDate: 'Y-m-d',
                step: 5,
                inline: true,
                onChangeDateTime: function (dp, $input) {
                    $(this).val($input.val());
                }
            });
        }

    });

});
function dateConvert(value) {
    var datetime = value.split(' ');
    var datevalues = datetime[0].split('-');
    var timevalues = datetime[1].split(':');

    if (datevalues[0].length > 2) {
        return value;
    }

    var year = datevalues[2];
    
    var month = datevalues[1];
    if (month <= 9)
        month = '0' + month;
   
    var day = datevalues[0];
    if (day <= 9)
        day = '0' + day;

    return year + '/' + month + '/' + day + " " + timevalues[0] + ":" + timevalues[1];
}