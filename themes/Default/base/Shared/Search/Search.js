$(document).ready(function () {

    var $search_form = $('<form>')
                    .prop('id', 'Search')
                    .prop('method', 'GET')
                    .prop('action', globals.prefix + 'html/Navigation/Search.html');

    var $search_text = $('<input>')
                    .prop('id', 'tipue_search_input')
                    .prop('type', 'text')
                    .prop('name', 'q')
                    .prop('autocomplete', 'off')
                    .prop('required', true)
                    .prop('value', 'Search...')
                    .focus(function () { if ($(this).prop('value') === 'Search...') { $(this).prop('value', ''); } })
                    .focusout(function () { if ($(this).prop('value') === '') { $(this).prop('value', 'Search...'); } });

    var $search_submit = $('<input>')
                    .prop('id', 'Submit')
                    .prop('type', 'submit')
                    .prop('value', '');

    var $search_results = $('<div>')
                    .prop('id', 'tipue_search_content');

    var $search = $search_form.append($search_text).append($search_submit);

    $search.appendTo("#Browse");

    $(document).ready(function () {
        $('#tipue_search_input').tipuesearch(
              {
                  'showURL': false,
                  'autocomplete': {
                      categorized: true,
                      minimumLength: 2
                  }
              });
    });
})