import './ActiveSpells.css'
import React from 'react'
import EntryList from '../../core/EntryList'

class ActiveSpell extends React.Component
{
	render()
	{
		return (<div className="entry activeSpell">
			<input type="text" className="spell" value={this.props.spell} onChange={(event) => this.props.changeHandler(this.props.index, event.target.value)} />
		</div>)
	}
}

class ActiveSpells extends EntryList
{
	render()
	{
		let entries = this.generateEntries()
		
		return (
			<div className="entryList">
				<div className="entryListLabel">{this.props.title}</div>
				{entries}
			</div>
		)
	}
	
	generateEntries()
	{
		let entries = []
		
		this.props.activeSpells.forEach((spell, i) => {
			entries.push(<ActiveSpell key={`active-spell-${i}`} index={i} spell={spell.spell} max={this.props.max} changeHandler={(index, value) => this.props.changeHandler(index, value)} />)
		})
		
		return entries
	}
}

export default ActiveSpells
