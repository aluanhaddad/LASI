﻿import $ from 'jquery';
import { LexicalModel } from 'app/models';

export default function buildMenus() {
  const contextualElementIdSelectors: string[] = [];
  const verbalMenuTextToElementsMap = {
    'View Subjects': 'subjects',
    'View Direct Objects': 'directObjects',
    'View Indirect Objects': 'indirectObjects'
  };
  const relationshipCssClassNameMap = {
    'View Subjects': 'subject-of-current',
    'View Direct Objects': 'direct-object-of-current',
    'View Indirect Objects': 'indirect-object-of-current'
  };
  return function () {
    const forVerbal = (context: { id: number }[]): { [key: string]: {}[] } => {

      const menu = JSON.parse($('#context' + context[0].id).text());
      const subjects = menu.subjects;
      const directObjects = menu.directObjects;
      const indirectObjects = menu.indirectObjects;

      return Object.entries({
        subjects,
        directObjects,
        indirectObjects
      }).filter(([, value]) => value).reduce((o, [key, value]) => ({ ...o, [key]: value }), {});
    };

    const forReferencer = (context: { id: number }[]): { referredTo: {}[] } => JSON.parse($('#context' + context[0].id).text());

    $('span.referencer').contextmenu({
      target: '#referencer-context-menu',
      before(event: Event & { target: { lexicalContextMenu: {} } }, context: {}) {
        const data = forReferencer(context as { id: number }[]);
        event.target.lexicalContextMenu = data;
        return data.referredTo && data.referredTo.length > 0;
      },
      onItem(context: [{ lexicalContextMenu: { referredTo: { id: number }[] } }]) {
        context[0].lexicalContextMenu.referredTo
          .map(id => $(`#${id}`))
          .forEach($e => $e.css('background-color', 'red'));
      }
    });

    $('span.verbal').contextmenu({
      target: '#verbal-context-menu',
      before(e: { target: { lexicalContextMenu: { [key: string]: {} } } }, context) {
        let count = 0;
        const menu = forVerbal(context);
        e.target.lexicalContextMenu = {};
        Object.entries(menu).forEach(([key, value = []]) => {
          e.target.lexicalContextMenu[key] = value.map(id => {
            const idSelector = '#' + id;
            if (!contextualElementIdSelectors.some(e => e === idSelector)) {
              contextualElementIdSelectors.push(idSelector);
            }
            return idSelector;
          });
        });
        [
          { name: 'subjects', id: '#subjects-item' },
          { name: 'directObjects', id: '#directobjects-item' },
          { name: 'indirectObjects', id: '#indirectobjects-item' }
        ].forEach(item => {
          if (!menu[item.name]) {
            $(item.id).hide();
          } else {
            count += 1;
            $(item.id).show();
          }
        });
        return count > 0;
      },
      onItem(context, event: Event & { target: { text: keyof typeof relationshipCssClassNameMap } }) {
        const menu = context[0].lexicalContextMenu;
        contextualElementIdSelectors
          .flatMap(e => $(e).toArray(), $)
          .forEach($e => {
            Object.values(relationshipCssClassNameMap).forEach(relationshipCssClassName => {
              $e.removeClass(relationshipCssClassName);
            });
          });
        menu[verbalMenuTextToElementsMap[event.target.text]]
          .map($)
          .forEach($e => {
            $e.addClass(relationshipCssClassNameMap[event.target.text]);
          });

      }
    });
    $(document).on('click', () => {
      $('#verbal-context-menu').hide();
      $('#referencer-context-menu').hide();
    });
  };
}