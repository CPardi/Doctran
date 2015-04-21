$(document).ready(function()
{
	$('#Contents-Menu').hide();
	$('#Breadcrumbs').hide();
})

function View_onclick(id_name) {
	rememberView(id_name)
	return true;
}