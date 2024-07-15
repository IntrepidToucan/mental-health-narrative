EXTERNAL hasItem(id)
EXTERNAL updateAffinity(delta)
EXTERNAL updateInventory(itemId, delta)

CONST ITEM_EROL_BLOOD_SAMPLE = "ErolBloodSample"
CONST ITEM_EROL_SERVICEMANS_BADGE = "ErolServicemansBadge"

-> main

== main ==
{
    - hasItem(ITEM_EROL_BLOOD_SAMPLE):
        -> has_blood_sample
    - hasItem(ITEM_EROL_SERVICEMANS_BADGE):
        -> intro_breakthrough
    - else:
        -> intro_dead_end
}

== intro_dead_end ==
...
+ [Uh... Hello?]
+ [Erol?]
+ [I need something from you.]
+ [I’m here to help.]

- ...
-> END

== intro_breakthrough ==
...
+ [At ease.]

- ‘E finks e’s a jokah...
+ [Wasn’t joking. You look tired.]
An’ e’s a bloody doctor, too. Anyfing ‘e can’t do?

- (choices)
+ (maze) {maze < 2} [{Can’t figure my way around this building, to be honest!|About this maze...}]
{Heh... Ain’t that th’truth. Bloody maze...|Hrm, wot about it?}
    ** [Do you want to get out of here?]
    Ain’t no way out for me. Leave if y’like.
    -> choices
    ++ [I think mazes are fun.]
    You an’ me got diff’rent ideas of “fun.”
        *** [What do you do for fun?]
        Mind my own business. Y’startin’ t’get on my nerves...
            ---- (maze_choices)
            **** [You’re touchy for a “big man.”]
            I’d be leaving now, Sunshine... {updateAffinity(-20)}
            -> maze_choices
            **** [Anything I can do to make it up to you?]
            ...
            Get me my post.
            Should be a paper there. Ain’t read nofink since I been stuck ‘ere.
            -> END
            **** [Fair enough.]
            ...
            -> choices
+ [Can’t watch you holding all that up. Let me show you how it’s done.]
Fink y’got ‘alf a chance a’this, Sunshine?
-> END
+ [Don’t waste my time. Look, I’m here for a blood sample.]
Y’gonna leave me to it after all that?
Fine. Take what y’like, just leave me alone. {updateInventory(ITEM_EROL_BLOOD_SAMPLE, 1)}
-> END

== has_blood_sample ==
Y'got what y'came for. Leave me alone.
-> END